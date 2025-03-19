using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 스프라이트가 이동한다(수평으로) 
public class OnKeyPress_MoveSpriteH : MonoBehaviour
{

    public float speed = 0.7f; // 속도 : Inspector로 지정

    private float vx = 0f;
    private float vy = 0f;

    void Update() { // 계속 시행한다
        vx = 0f;
        vy = 0f;
        if (Input.GetKey("right"))// 만약 오른쪽 키가 눌리면
		{ 
            vx = speed; // 오른쪽으로 진행하는 이동량을 넣는다 
        }
        if (Input.GetKey("left"))// 만약 왼쪽 키가 눌리면
		{  
            vx = -speed; // 왼쪽으로 진행하는 이동량을 넣는다 
        }
    }
    void FixedUpdate()// 계속 시행한다(일정 시간마다) 
	{ 
        this.transform.Translate(vx / 50f, vy / 50f, 0);
    }
}
