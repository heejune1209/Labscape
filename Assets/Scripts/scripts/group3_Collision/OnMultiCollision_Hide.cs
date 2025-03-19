using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 지운다(여러 개 대응) 
public class OnMultiCollision_Hide : MonoBehaviour
{

	public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
	public string hideObjecttName;  // 지우는 오브젝트 이름 : Inspector에 지정

	GameObject hideObject;
	float orgY = 0;
	float ofsetY = 10000; // 이 값을 Y방향에 더하는 것으로 사라진다 

	void Start()
	{ // 처음에 시행한다 
		// 지우는 오브젝트를 찾아 놓는다 
		hideObject = GameObject.Find(hideObjecttName);
		orgY = hideObject.transform.position.y;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{ // 충돌했을 때 
		// 만약 충돌한 것의 이름이 목표 오브젝트면 
		if (collision.gameObject.name == targetObjectName)
		{
			// 지우는 【hideObject.SetActive(false); 대신】
			Vector3 pos = hideObject.transform.position;
			pos.y = orgY + ofsetY;
			hideObject.transform.position = pos;
		}
	}
}
