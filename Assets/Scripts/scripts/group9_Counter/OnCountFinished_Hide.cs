using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운터가 최종값이면 지운다 
public class OnCountFinished_Hide : MonoBehaviour
{

	public int lastCount = 3; // 카운터의 최종값 : Inspector에 지정
	public string hideObjectName;  // 지우는 오브젝트 이름 : Inspector에 지정

	void Start()
	{
        // 처음에는 아무 것도 하지 않는다 
	}

	void FixedUpdate()// 계속 시행한다(일정 시간마다)
	{  
		// 카운터가 최종값이 되면 
		if (GameCounter.value == lastCount)
		{
			// 지우는 오브젝트를 찾아서 
			GameObject hideObject = GameObject.Find(hideObjectName);
			hideObject.SetActive(false); // 지운다
		}
	}
}
