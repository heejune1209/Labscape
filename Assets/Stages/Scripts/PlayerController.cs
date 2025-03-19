using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;  // ĳ������ �̵� �ӵ�
    float moveHorizontal = 0f;
    public float jumpForce = 6.0f;  // ���� ��
    Animator anim;
    Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private int jumpCount = 0;  // ���� ���� Ƚ��
    public int maxJumpCount = 1;  // �ִ� ���� Ƚ�� (1ȸ ���� ���� + 1ȸ ���� ����)

    private bool isFeatherActive = false; // ���� ������ Ȱ��ȭ ����
    private bool isShoesActive = false; // �Ź� ������ Ȱ��ȭ ����
    private bool isHourglassActive = false;  // �𷡽ð� ������ Ȱ��ȭ ����
    [SerializeField]
    private float featherTimer = 6f; // ���� ������ ���� �ð� Ÿ�̸�
    [SerializeField]
    private float ShoesTimer = 6f; // �Ź� ������ ���� �ð� Ÿ�̸�
    [SerializeField]
    private float HourglassTimer = 2.5f; // �𷡽ð� ������ ���� �ð� Ÿ�̸�
    [SerializeField]
    private float Speedchange = 1.5f; // ������ ���� ��ȭ ���
    [SerializeField]
    private float Jumpchange = 1.5f; // ������ ���� ������ ��ȭ ���
    private float normalSpeed; // ���� �ӵ� ����
    private float normalJump; // ���� ������ ����
    private float originalGravityScale; // ���� �߷� �� ����

    private BoxCollider2D boxCollider;  // �÷��̾��� �ڽ� �ݶ��̴�
    public float deathDelay = 3.0f;  // ���� �� ����� �� ��� �ð�
    private bool isDead = false;  // �÷��̾��� ��� ����    
    [HideInInspector]
    public bool canMove = true; // �̵� ���� ����
    public DeathUI deathUI; // DeathUIController ����

    private void Start()
    {
        Respawn();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();  // Rigidbody ������Ʈ�� ������
        jumpCount = 0;
        normalSpeed = speed; // ���� ���� �� ���� �ӵ� ����
        normalJump = jumpForce; // ���� ���� �� ���� ������ ����
        originalGravityScale = rb.gravityScale; // ���� ���� �� ���� �߷� �� ����
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
            rb.velocity = Vector2.zero; // �ӵ� 0���� ����
            canMove = false; // �̵� �Ұ���
        }
        else
        {
            GetComponent<PlayerController>().enabled = true;
             
        }
    }

    private void HandleMovement() // �̵� �Է��� ó���ϰ�, ĳ������ �̵� �� ���� ���¸� ����
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
            anim.SetBool("isRun", true); // ĳ���Ͱ� ������ �� isRun�� true�� ����
        }
        else
        {
            StopMoving();
            anim.SetBool("isRun", false); // ĳ���Ͱ� ���� �� isRun�� false�� ����
        }
    }

    private void MoveCharacter(float direction) // ĳ������ ���� �̵��� ó���ϸ�, HandleRotation�� ȣ���Ͽ� ���� ��ȯ�� ����
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        HandleRotation(direction);
    }

    private void StopMoving() // ĳ������ �ӵ��� 0���� ����
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void HandleRotation(float direction) // ĳ������ �������� �����Ͽ� ������ �ٲߴϴ�. �̴� �¿� ������ ���� ����
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
            isMovingRight = false; // �������� �̵��� �� ������ �̵� ����
        }
    }

    public void MoveRight(bool isPressed)
    {
        isMovingRight = isPressed;
        if (isPressed)
        {
            isMovingLeft = false; // ���������� �̵��� �� ���� �̵� ����
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
        // �̹� ���� ���¸� �޼ҵ� ������ ����
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Die");
        boxCollider.size = new Vector2(boxCollider.size.y, boxCollider.size.x);
        boxCollider.offset = new Vector2(boxCollider.offset.y, boxCollider.offset.x);
        deathUI.gameObject.SetActive(true); // DeathUIController ������Ʈ Ȱ��ȭ
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
            // �⺻ ��ġ�� ������ (���ϴ� �⺻ ��ġ ����)
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
        canMove = true; // �̵� ����
        rb.gravityScale = originalGravityScale; // ���� �߷� ������ ����
        gameObject.tag = "Player"; // �±׸� ������� ����
        //anim.enabled = true; // �ִϸ��̼� �ٽ� Ȱ��ȭ
    }

    public void ActiveHourGlass()
    {
        if (!isHourglassActive)
        {
            isHourglassActive = true;
            canMove = false; // �̵� �Ұ���
            rb.velocity = Vector2.zero; // �ӵ� 0���� ����
            rb.gravityScale = 0f; // �߷� ��Ȱ��ȭ
            // ������ ��ֹ��̶� ������� ���ø� ���ؼ� �� �ڵ�
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), false);
            anim.SetBool("isRun", false); // �޸��� �ִϸ��̼� ����

            gameObject.tag = "Invincibility"; // �±׸� Invincibility�� ����
            //anim.enabled = false; // �ִϸ��̼� ��Ȱ��ȭ
            StartCoroutine(HourglassInvincibility(HourglassTimer)); // ���� ���� �ڷ�ƾ ����
        }
    }

    private IEnumerator HourglassInvincibility(float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateHourglass();
    }
}