using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 이동한다
public class OnKeyPress_Move : MonoBehaviour
{

    public float speed = 2; // 속도：Inspector에 지정

    float vx = 0;
    float vy = 0;
    bool leftFlag = false;
    Rigidbody2D rbody;

    void Start()
    { // 처음에 시행한다
      // 중력을 0으로 해서 충돌 시에 회전시키지 않는다
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    { // 계속 시행한다
        vx = 0;
        vy = 0;
        if (Input.GetKey("right"))
        { // 만약 오른쪽 키가 눌리면
            vx = speed; // 오른쪽으로 나아가는 이동량을 넣는다
            leftFlag = false;
        }
        if (Input.GetKey("left"))
        { // 만약 왼쪽 키가 눌리면
            vx = -speed; // 왼쪽으로 나아가는 이동량을 넣는다
            leftFlag = true;
        }
        if (Input.GetKey("up"))
        { // 만약 위쪽 키가 눌리면
            vy = speed; // 위 방향으로 나아가는 이동량을 넣는다
        }
        if (Input.GetKey("down"))
        { // 만약 아래 키가 눌리면
            vy = -speed; // 아래로 나아가는 이동량을 넣는다
        }
    }
    void FixedUpdate()
    { // 계속 시행한다(일정 시간마다)
      // 이동한다
        rbody.velocity = new Vector2(vx, vy);
        // 왼쪽 오른쪽 방향을 바꾼다
        this.GetComponent<SpriteRenderer>().flipX = leftFlag;
    }
}
