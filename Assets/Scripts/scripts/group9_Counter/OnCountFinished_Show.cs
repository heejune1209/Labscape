using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운터가 최종값이면 표시한다 
public class OnCountFinished_Show : MonoBehaviour
{
	
	public int lastCount = 3; // 카운터의 최종값 : Inspector에 지정
	public string showObjectName;  // 표시 오브젝트 이름：Inspector에 지정

	GameObject showObject;

	void Start () // 처음에 시행한다 
	{ 
		// 지우기 전에 표시 오브젝트를 기억해 둔다 
		showObject = GameObject.Find(showObjectName);
		showObject.SetActive(false); // 지운다
	}

	void FixedUpdate() // 계속 시행한다(일정 시간마다)
	{ 
		// 카운터가 최종값이 되면 
		if (GameCounter.value == lastCount) {
			showObject.SetActive(true); // 지웠던 것을 표시한다 
		}
	}
}
