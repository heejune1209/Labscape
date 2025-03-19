using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 이동한다(중력 대응판) 
public class OnKeyPress_MoveGravity : MonoBehaviour
{

    public float speed = 3; // 속도：Inspector에 지정
    public float jumppower = 8;  // 점프력：Inspector에 지정

    float vx = 0;
    bool leftFlag = false; // 왼쪽 방향인지 
    bool pushFlag = false; // 스페이스 키가 눌린 상태인지 
    bool jumpFlag = false; // 점프 상태인지 
    bool groundFlag = false; // 발이 무언가에 닿았는지 
    Rigidbody2D rbody;

    void Start ()// 처음에 시행한다
    { 
        // 충돌 시에 회전시키지 않는다 
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update () // 계속 시행한다 
    { 
        vx = 0;
        if (Input.GetKey("right")) // 만약 오른쪽 키가 눌리면
        { 
            vx = speed; // 오른쪽으로 진행하는 이동량을 넣는다
            leftFlag = false;
        }
        if (Input.GetKey("left")) // 만약 왼쪽 키가 눌리면
        { // 
            vx = -speed; // 왼쪽으로 진행하는 이동량을 넣는다 
            leftFlag = true;
        }
        // 만약 스페이스키가 눌렸을 때 발이 무언가에 닿았다면 
        if (Input.GetKey("space") && groundFlag)
        { 
            if (pushFlag == false)// 계속 누르고 나가지 않으면
            { 
                jumpFlag = true; // 점프 준비 
                pushFlag = true; // 계속 누른 상태 
            }
        } else
        {
            pushFlag = false; // 계속 누른 상태 해제 
        }
    }
    void FixedUpdate() // 계속 시행한다(일정 시간마다)
    { 
        // 이동한다(중력을 건 채)
        rbody.velocity = new Vector2(vx, rbody.velocity.y);
        // 왼쪽 오른쪽으로 방향을 바꾼다 
        this.GetComponent<SpriteRenderer>().flipX = leftFlag;
        // 만약 점프할 때 
        if (jumpFlag) {
            jumpFlag = false;
            rbody.AddForce(new Vector2(0, jumppower), ForceMode2D.Impulse);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    { // 발이 무언가에 닿으면 
        groundFlag = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    { // 발에 아무 것도 닿지 않으면 
        groundFlag = false;
    }
}