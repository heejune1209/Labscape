using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 이동한다(중력대응 테스트판)
public class OnKeyPress_TestMoveGravity : MonoBehaviour
{

    public float speed = 3; // 속도：Inspector에 지정
    public float jumppower = 8;  // 점프 힘 ：Inspector에 지정

    float vx = 0;
    bool leftFlag = false; // 왼쪽 방향인지 여부
    bool pushFlag = false; // 스페이스키를 누르고 있는지 여부
    bool jumpFlag = false; // 점프 상태인지 여부
    Rigidbody2D rbody;

    void Start() // 처음에 시행한다 
    { 
      // 충돌 시에 회전시키지 않는다
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()// 계속 시행한다
    { 
        vx = 0;
        if (Input.GetKey("right")) // 만약 오른쪽 키가 눌리면
        { 
            vx = speed; // 오른쪽으로 진행하는 이동량을 넣는다
            leftFlag = false;
        }
        if (Input.GetKey("left"))// 만약 왼쪽 키가 눌리면
        { 
            vx = -speed; // 왼쪽으로 진행하는 이동량을 넣는다
            leftFlag = true;
        }
        // 만약 스페이스키가 눌리면
        if (Input.GetKey("space"))
        {
            if (pushFlag == false)// 누르고 있지 않으면
            { 
                jumpFlag = true; // 점프 준비
                pushFlag = true; // 누르고 있는 상태
            }
        }
        else
        {
            pushFlag = false; // 누르고 있음 해제
        }
    }
    void FixedUpdate()// 계속 시행한다(일정 시간마다)
    { 
      // 이동한다(중력을 가한 채)
        rbody.velocity = new Vector2(vx, rbody.velocity.y);
        // 왼쪽 오른쪽으로 방향을 바꾼다
        this.GetComponent<SpriteRenderer>().flipX = leftFlag;
        // 만약 점프할 때
        if (jumpFlag)
        {
            jumpFlag = false;
            rbody.AddForce(new Vector2(0, jumppower), ForceMode2D.Impulse);
        }
    }
}
