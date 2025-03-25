using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // 플레이어 이동 속도
    public float speed = 3.0f;
    // 좌우 이동 입력 값 (왼쪽: -1, 오른쪽: 1)
    float moveHorizontal = 0f;
    // 점프 힘
    public float jumpForce = 6.0f;
    // 애니메이터 컴포넌트 참조
    Animator anim;
    // Rigidbody2D 컴포넌트 참조
    Rigidbody2D rb;
    // 바닥에 닿았는지 여부
    private bool isGrounded = true;
    // 왼쪽 이동 중인지 여부
    private bool isMovingLeft = false;
    // 오른쪽 이동 중인지 여부
    private bool isMovingRight = false;
    // 현재 점프 횟수
    private int jumpCount = 0;
    // 최대 점프 횟수 (예: 더블 점프 등)
    public int maxJumpCount = 1;

    // 깃털 아이템 효과 활성화 여부
    private bool isFeatherActive = false;
    // 신발(점프 강화) 아이템 효과 활성화 여부
    private bool isShoesActive = false;
    // 모래시계(시간 정지) 아이템 효과 활성화 여부
    private bool isHourglassActive = false;
    // 깃털 효과 지속 시간
    [SerializeField]
    private float featherTimer = 6f;
    // 신발 효과 지속 시간
    [SerializeField]
    private float ShoesTimer = 6f;
    // 모래시계 효과 지속 시간
    [SerializeField]
    private float HourglassTimer = 2.5f;
    // 깃털 아이템 적용 시 속도 변경 배수
    [SerializeField]
    private float Speedchange = 1.5f;
    // 신발 아이템 적용 시 점프 힘 변경 배수
    [SerializeField]
    private float Jumpchange = 1.5f;
    // 정상 이동 속도 저장
    private float normalSpeed;
    // 정상 점프 힘 저장
    private float normalJump;
    // Rigidbody의 중력 스케일 기본 값 저장
    private float originalGravityScale;

    // 플레이어의 충돌 처리를 위한 BoxCollider2D 참조
    private BoxCollider2D boxCollider;
    // 사망 후 재시작까지의 지연 시간
    public float deathDelay = 3.0f;
    // 플레이어 사망 여부
    private bool isDead = false;
    // 플레이어가 움직일 수 있는지 여부
    [HideInInspector]
    public bool canMove = true;
    // 사망 UI 관련 스크립트 참조
    public DeathUI deathUI;

    // 초기화: 시작 시 플레이어 상태, 컴포넌트, 변수 값 설정
    private void Start()
    {
        // 리스폰 위치로 이동 (저장된 위치가 있다면)
        Respawn();
        // 애니메이터 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
        // 정상 속도, 점프 힘, 중력 스케일 값 저장
        normalSpeed = speed;
        normalJump = jumpForce;
        originalGravityScale = rb.gravityScale;
        // BoxCollider2D 컴포넌트 가져오기
        boxCollider = GetComponent<BoxCollider2D>();
        isDead = false;
        isHourglassActive = false;
        // 플레이어와 Block, Elevator 레이어 간 충돌을 허용 (false: 충돌 무시 안함)
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Elevator"), false);
    }

    // 매 프레임마다 호출되는 Update 메서드
    void Update()
    {
        // 이동 가능한 상태일 때 입력 처리
        if (canMove)
        {
            HandleMovement();
        }

        // 깃털 아이템 효과가 활성화된 경우, 타이머 감소 후 효과 해제
        if (isFeatherActive)
        {
            featherTimer -= Time.deltaTime;
            if (featherTimer <= 0)
            {
                DeactivateFeather();
            }
        }

        // 신발 아이템 효과가 활성화된 경우, 타이머 감소 후 효과 해제
        if (isShoesActive)
        {
            ShoesTimer -= Time.deltaTime;
            if (ShoesTimer <= 0)
            {
                DeactivateShoes();
            }
        }

        // 플레이어가 사망한 경우, 이동 및 입력 비활성화
        if (isDead)
        {
            // 자기 자신(PlayerController) 컴포넌트 비활성화
            GetComponent<PlayerController>().enabled = false;
            rb.velocity = Vector2.zero; // 이동 속도 0으로 설정
            canMove = false; // 이동 불가능 처리
        }
        else
        {
            // 사망하지 않은 경우, 컴포넌트 활성화
            GetComponent<PlayerController>().enabled = true;
        }
    }

    // 이동 입력을 처리하는 메서드 (좌우 이동)
    private void HandleMovement()
    {
        // 왼쪽 이동 입력이면 -1, 오른쪽이면 1, 둘 다 아니면 0 설정
        if (isMovingLeft)
        {
            moveHorizontal = -1f;
        }
        else if (isMovingRight)
        {
            moveHorizontal = 1f;
        }
        else
        {
            moveHorizontal = 0f;
        }

        // 입력이 있을 경우 캐릭터 이동 및 애니메이션 재생
        if (moveHorizontal != 0)
        {
            MoveCharacter(moveHorizontal);
            anim.SetBool("isRun", true); // 달리기 애니메이션 활성화
        }
        else
        {
            StopMoving();
            anim.SetBool("isRun", false); // 달리기 애니메이션 비활성화
        }
    }

    // 주어진 방향으로 캐릭터를 이동시키고, 방향 전환 처리
    private void MoveCharacter(float direction)
    {
        // 속도에 따라 Rigidbody의 속도 변경 (x축만)
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        // 캐릭터가 바라보는 방향을 처리
        HandleRotation(direction);
    }

    // 캐릭터 이동 정지 (x축 속도를 0으로)
    private void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    // 이동 방향에 따라 캐릭터의 좌우 회전을 처리
    private void HandleRotation(float direction)
    {
        // 이동 방향과 현재 스케일의 부호가 다르면 x 스케일을 반전시켜 캐릭터의 방향 전환
        if ((direction < 0 && transform.localScale.x > 0) || (direction > 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // UI 버튼 등을 통해 왼쪽 이동 입력을 처리하는 메서드
    public void MoveLeft(bool isPressed)
    {
        isMovingLeft = isPressed;
        if (isPressed)
        {
            // 왼쪽 입력이 활성화되면 오른쪽 입력은 취소
            isMovingRight = false;
        }
    }

    // UI 버튼 등을 통해 오른쪽 이동 입력을 처리하는 메서드
    public void MoveRight(bool isPressed)
    {
        isMovingRight = isPressed;
        if (isPressed)
        {
            // 오른쪽 입력이 활성화되면 왼쪽 입력은 취소
            isMovingLeft = false;
        }
    }

    // 점프 입력을 처리하는 메서드
    public void Jump()
    {
        // 이동 가능하며, 땅에 닿아 있거나 점프 횟수가 최대치보다 작을 때 점프
        if (canMove && (isGrounded || jumpCount < maxJumpCount))
        {
            // 위쪽 방향으로 점프 힘을 적용 (Impulse 모드)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpCount++;
            anim.SetBool("isJumping", true); // 점프 애니메이션 활성화
        }
    }

    // 충돌 시작 시 호출 (2D 물리 충돌)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Ground" 태그와 충돌하면 바닥에 닿은 것으로 처리
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            anim.SetBool("isJumping", false); // 점프 애니메이션 비활성화
        }
    }

    // 충돌 종료 시 호출
    private void OnCollisionExit2D(Collision2D collision)
    {
        // "Ground"와의 충돌이 끝나면 바닥에서 떨어진 것으로 처리
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // 트리거 영역에 들어갔을 때 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // DeathZone 태그와 충돌하면 사망 처리 (이미 사망하지 않은 경우)
        if (collision.gameObject.tag == "DeathZone" && !isDead)
        {
            Die();
        }
        // Spanner(아이템) 태그와 충돌하면 아이템 획득 처리
        if (collision.gameObject.tag == "Spanner")
        {
            GameManager.Instance.AddSpanner();
            AudioManager.instance.PlaySFX(3);
            Destroy(collision.gameObject);
        }
    }

    // 사망 처리 메서드
    public void Die()
    {
        // 이미 사망한 경우 중복 처리 방지
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Die"); // 사망 애니메이션 트리거
        // 박스 콜라이더 크기와 오프셋을 변경하여 사망 상태 표현 (예시)
        boxCollider.size = new Vector2(boxCollider.size.y, boxCollider.size.x);
        boxCollider.offset = new Vector2(boxCollider.offset.y, boxCollider.offset.x);
        // 사망 UI 활성화 및 사망 후 처리 호출
        deathUI.gameObject.SetActive(true);
        deathUI.OnPlayerDeath();
        AudioManager.instance.PlaySFX(11);
        // 일정 시간 후 게임 재시작
        StartCoroutine(RestartGameAfterDelay(deathDelay));
        GameManager.Instance.PlayerDie();
    }

    // 일정 시간 지연 후 현재 씬을 다시 로드하는 코루틴
    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 리스폰 위치가 저장되어 있다면 해당 위치로 이동, 없으면 현재 위치 유지
    private void Respawn()
    {
        if (PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY") && PlayerPrefs.HasKey("RespawnZ"))
        {
            float x = PlayerPrefs.GetFloat("RespawnX");
            float y = PlayerPrefs.GetFloat("RespawnY");
            float z = PlayerPrefs.GetFloat("RespawnZ");
            transform.position = new Vector3(x, y, z);
        }
        else
        {
            // 저장된 리스폰 위치가 없으면 현재 위치 유지
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    // 깃털 아이템 활성화 메서드: 속도 증가 효과 적용
    public void ActivateFeather()
    {
        if (!isFeatherActive)
        {
            isFeatherActive = true;
            speed *= Speedchange; // 속도 증가
            featherTimer = 6.0f;  // 타이머 초기화
        }
    }

    // 깃털 효과 비활성화: 속도를 원래대로 복원
    private void DeactivateFeather()
    {
        isFeatherActive = false;
        speed = normalSpeed;
    }

    // 신발 아이템 활성화 메서드: 점프 힘 증가 효과 적용
    public void ActivateShoes()
    {
        if (!isShoesActive)
        {
            isShoesActive = true;
            jumpForce *= Jumpchange; // 점프 힘 증가
            ShoesTimer = 6.0f;       // 타이머 초기화
        }
    }

    // 신발 효과 비활성화: 점프 힘을 원래대로 복원
    private void DeactivateShoes()
    {
        isShoesActive = false;
        jumpForce = normalJump;
    }

    // 모래시계 효과 비활성화: 이동 가능 상태 복원, 중력 복원, 태그 변경
    private void DeactivateHourglass()
    {
        isHourglassActive = false;
        canMove = true; // 이동 가능
        rb.gravityScale = originalGravityScale; // 중력 복원
        gameObject.tag = "Player"; // 태그를 원래대로 복원
        // anim.enabled = true; // 필요시 애니메이터 재활성화 (주석 처리됨)
    }

    // 모래시계(시간 정지) 아이템 활성화 메서드
    public void ActiveHourGlass()
    {
        if (!isHourglassActive)
        {
            isHourglassActive = true;
            canMove = false; // 이동 불가 처리
            rb.velocity = Vector2.zero; // 현재 속도 0으로 설정
            rb.gravityScale = 0f; // 중력 효과 제거
            // Block 레이어와의 충돌 무시 설정 (false로 설정되어 있으므로 충돌 발생)
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), false);
            anim.SetBool("isRun", false); // 달리기 애니메이션 비활성화

            gameObject.tag = "Invincibility"; // 태그를 무적 상태로 변경
            // anim.enabled = false; // 필요시 애니메이터 비활성화 (주석 처리됨)
            // 모래시계 효과 지속 시간 동안 Invincibility 상태 유지 후 비활성화 처리
            StartCoroutine(HourglassInvincibility(HourglassTimer));
        }
    }

    // 모래시계 효과 유지 후 Invincibility 상태를 해제하는 코루틴
    private IEnumerator HourglassInvincibility(float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateHourglass();
    }
}