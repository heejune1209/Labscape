using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 애니메이션을 재생&정지 
public class OnMouseDown_StartStop : MonoBehaviour
{

	public float speed = 0;

	void Start()// 처음에 시행한다. 정지
	{ 
		this.GetComponent<Animator>().speed = speed;
	}
	void OnMouseDown()// 터치하면
	{ 
		speed = 1 - speed; // 재생과 정지를 전환한다 
		this.GetComponent<Animator>().speed = speed;
	}
}
