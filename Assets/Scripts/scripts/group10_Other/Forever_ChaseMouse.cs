using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속 마우스를 쫓아간다 
public class Forever_ChaseMouse : MonoBehaviour
{

	void Start ()
	{
		// 외관은 앞쪽으로
		this.GetComponent<SpriteRenderer>().sortingOrder = 100;
	}

	void Update ()
	{
		Vector3 position = Input.mousePosition;
		position.z = 10f; // 다른 것의 터치에 영향없도록 안으로 
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(position);
	}
}
