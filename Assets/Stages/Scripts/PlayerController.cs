using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;  // 캐릭터의 이동 속도
    float moveHorizontal = 0f;
    public float jumpForce = 6.0f;  // 점프 힘
    Animator anim;
    Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private int jumpCount = 0;  // 현재 점프 횟수
    public int maxJumpCount = 1;  // 최대 점프 횟수 (1회 지상 점프 + 1회 공중 점프)

    private bool isFeatherActive = false; // 깃털 아이템 활성화 여부
    private bool isShoesActive = false; // 신발 아이템 활성화 여부
    private bool isHourglassActive = false;  // 모래시계 아이템 활성화 여부
    [SerializeField]
    private float featherTimer = 6f; // 깃털 아이템 지속 시간 타이머
    [SerializeField]
    private float ShoesTimer = 6f; // 신발 아이템 지속 시간 타이머
    [SerializeField]
    private float HourglassTimer = 2.5f; // 모래시계 아이템 지속 시간 타이머
    [SerializeField]
    private float Speedchange = 1.5f; // 아이템 사용시 변화 계수
    [SerializeField]
    private float Jumpchange = 1.5f; // 아이템 사용시 점프력 변화 계수
    private float normalSpeed; // 원래 속도 저장
    private float normalJump; // 원래 점프력 저장
    private float originalGravityScale; // 원래 중력 값 저장

    private BoxCollider2D boxCollider;  // 플레이어의 박스 콜라이더
    public float deathDelay = 3.0f;  // 죽은 후 재시작 전 대기 시간
    private bool isDead = false;  // 플레이어의 사망 상태    
    [HideInInspector]
    public bool canMove = true; // 이동 가능 여부
    public DeathUI deathUI; // DeathUIController 참조

    private void Start()
    {
        Respawn();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();  // Rigidbody 컴포넌트를 가져옴
        jumpCount = 0;
        normalSpeed = speed; // 게임 시작 시 원래 속도 저장
        normalJump = jumpForce; // 게임 시작 시 원래 점프력 저장
        originalGravityScale = rb.gravityScale; // 게임 시작 시 원래 중력 값 저장
        boxCollider = GetComponent<BoxCollider2D>();
        isDead = false;
        isHourglassActive = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Elevator"), false);
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }

        if (isFeatherActive)
        {
            featherTimer -= Time.deltaTime;
            if (featherTimer <= 0)
            {
                DeactivateFeather();
            }
        }

        if (isShoesActive)
        {
            ShoesTimer -= Time.deltaTime;
            if (ShoesTimer <= 0)
            {
                DeactivateShoes();
            }
        }
        
        if (isDead)
        {
            GetComponent<PlayerController>().enabled = false;
            rb.velocity = Vector2.zero; // 속도 0으로 설정
            canMove = false; // 이동 불가능
        }
        else
        {
            GetComponent<PlayerController>().enabled = true;
             
        }
    }

    private void HandleMovement() // 이동 입력을 처리하고, 캐릭터의 이동 및 멈춤 상태를 관리
    {
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

        if (moveHorizontal != 0)
        {
            MoveCharacter(moveHorizontal);
            anim.SetBool("isRun", true); // 캐릭터가 움직일 때 isRun을 true로 설정
        }
        else
        {
            StopMoving();
            anim.SetBool("isRun", false); // 캐릭터가 멈출 때 isRun을 false로 설정
        }
    }

    private void MoveCharacter(float direction) // 캐릭터의 실제 이동을 처리하며, HandleRotation을 호출하여 방향 전환을 관리
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        HandleRotation(direction);
    }

    private void StopMoving() // 캐릭터의 속도를 0으로 설정
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void HandleRotation(float direction) // 캐릭터의 스케일을 조정하여 방향을 바꿉니다. 이는 좌우 반전을 통해 구현
    {
        if ((direction < 0 && transform.localScale.x > 0) || (direction > 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void MoveLeft(bool isPressed)
    {
        isMovingLeft = isPressed;
        if (isPressed)
        {
            isMovingRight = false; // 왼쪽으로 이동할 때 오른쪽 이동 중지
        }
    }

    public void MoveRight(bool isPressed)
    {
        isMovingRight = isPressed;
        if (isPressed)
        {
            isMovingLeft = false; // 오른쪽으로 이동할 때 왼쪽 이동 중지
        }
    }

    public void Jump()
    {
        if (canMove && (isGrounded || jumpCount < maxJumpCount))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpCount++;
            anim.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            anim.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathZone" && !isDead)
        {
            Die();
        }
        if (collision.gameObject.tag == "Spanner")
        {
            GameManager.Instance.AddSpanner();
            AudioManager.instance.PlaySFX(3);
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        // 이미 죽은 상태면 메소드 실행을 중지
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Die");
        boxCollider.size = new Vector2(boxCollider.size.y, boxCollider.size.x);
        boxCollider.offset = new Vector2(boxCollider.offset.y, boxCollider.offset.x);
        deathUI.gameObject.SetActive(true); // DeathUIController 오브젝트 활성화
        deathUI.OnPlayerDeath();
        AudioManager.instance.PlaySFX(11);
        StartCoroutine(RestartGameAfterDelay(deathDelay));
        GameManager.Instance.PlayerDie();
    }

    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDead = false;        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
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
            // 기본 위치로 리스폰 (원하는 기본 위치 설정)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
    public void ActivateFeather()
    {
        if (!isFeatherActive)
        {
            isFeatherActive = true;
            speed *= Speedchange;
            featherTimer = 6.0f;
        }
    }

    private void DeactivateFeather()
    {
        isFeatherActive = false;
        speed = normalSpeed;
    }

    public void ActivateShoes()
    {
        if (!isShoesActive)
        {
            isShoesActive = true;
            jumpForce *= Jumpchange;
            ShoesTimer = 6.0f;
        }
    }

    private void DeactivateShoes()
    {
        isShoesActive = false;
        jumpForce = normalJump;
    }

    private void DeactivateHourglass()
    {
        isHourglassActive = false;
        canMove = true; // 이동 가능
        rb.gravityScale = originalGravityScale; // 원래 중력 값으로 복원
        gameObject.tag = "Player"; // 태그를 원래대로 변경
        //anim.enabled = true; // 애니메이션 다시 활성화
    }

    public void ActiveHourGlass()
    {
        if (!isHourglassActive)
        {
            isHourglassActive = true;
            canMove = false; // 이동 불가능
            rb.velocity = Vector2.zero; // 속도 0으로 설정
            rb.gravityScale = 0f; // 중력 비활성화
            // 스프링 장애물이랑 닿았을때 사용시를 위해서 쓴 코드
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), false);
            anim.SetBool("isRun", false); // 달리기 애니메이션 중지

            gameObject.tag = "Invincibility"; // 태그를 Invincibility로 변경
            //anim.enabled = false; // 애니메이션 비활성화
            StartCoroutine(HourglassInvincibility(HourglassTimer)); // 무적 상태 코루틴 시작
        }
    }

    private IEnumerator HourglassInvincibility(float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateHourglass();
    }
}