using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운터가 최종값이면 게임을 정지한다 
public class OnCountFinished_StopGame : MonoBehaviour
{

	public int lastCount = 3; // 카운터의 최종값 : Inspector에 지정
	
	void Start () // 처음에 시행한다
	{ 
		Time.timeScale = 1; // 시간을 움직인다
	}

	void FixedUpdate()// 계속 시행한다 (일정 시간마다) 
	{ 
		// 카운터가 최종값이 되면
		if (GameCounter.value == lastCount)
		{
			Time.timeScale = 0; // 시간을 멈춘다
		}
	}
}
