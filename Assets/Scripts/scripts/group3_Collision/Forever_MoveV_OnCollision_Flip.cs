using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속 이동하고, 충돌하면 반전한다(수직) 
public class Forever_MoveV_OnCollision_Flip : MonoBehaviour
{

	public float speed = 1; // 속도 ：Inspector에서 지정

	Rigidbody2D rbody;

	void Start ()// 처음에 시행한다
	{ 
		// 중력을 0으로 하고, 충돌 시에 회전시키지 않는다 
		rbody = GetComponent<Rigidbody2D>();
		rbody.gravityScale = 0;
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void FixedUpdate()// 계속 시행한다(일정 시간마다) 
	{ 
		// 수직으로 이동한다 
		rbody.velocity = new Vector2(0, speed); 
	}

	void OnCollisionEnter2D(Collision2D collision) // 충돌했을 때
	{
		speed = -speed;	// 진행하는 방향을 반전한다 
		// 나아가는 방향으로 위 아래 방향을 바꾼다 
		this.GetComponent<SpriteRenderer>().flipY = (speed < 0); 
	}
}
