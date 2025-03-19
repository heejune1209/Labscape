using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속 쫓아간다(중력 대응판) 
public class Forever_ChaseGravity : MonoBehaviour
{

	public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
	public float speed = 1; // 속도：Inspector에 지정

	GameObject targetObject;
	Rigidbody2D rbody;

	void Start () // 처음에 시행한다
	{  
		// 목표 오브젝트를 찾아둔다 
		targetObject = GameObject.Find(targetObjectName);
		// 충돌 시에 회전시키지 않는다 
		rbody = GetComponent<Rigidbody2D>();
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void FixedUpdate() // 계속 시행한다(일정 시간마다)
	{ 
		// 목표 오브젝트의 방향을 조사해서 
		Vector3 dir = (targetObject.transform.position - this.transform.position).normalized;
		// 그 방향으로 지정한 양으로 진행한다 (중력을 건 채로)
		float vx = dir.x * speed;
		rbody.velocity = new Vector2(vx, rbody.velocity.y);
		// 진행하는 방향에 왼쪽 오른쪽의 방향을 바꾼다 進む方向で左右の向きを変える
		SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
		sprite.flipX = (vx < 0); 
	}
}
