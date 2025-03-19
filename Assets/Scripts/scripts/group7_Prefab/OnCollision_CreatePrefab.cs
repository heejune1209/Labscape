using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 거기에 프리팹을 만든다 
public class OnCollision_CreatePrefab : MonoBehaviour
{

	public string targetObjectName; // 목표 오브젝트 이름 ：Inspector에 지정
	public GameObject newPrefab;    // 만드는 프리팹 : Inspector에 지정

	void OnCollisionEnter2D(Collision2D collision)// 충돌했을 때
	{  
		// 만약 충돌한 것의 이름이 목표 오브젝트면 
		if (collision.gameObject.name == targetObjectName)
		{
			// 충돌 위치에 프리팹을 만든다 
			GameObject newGameObject = Instantiate(newPrefab) as GameObject;
			Vector3 pos = collision.contacts[0].point;
			pos.z = -5; // 앞에 표시 
			newGameObject.transform.position = pos;
		}
	}
}
