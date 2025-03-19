using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 게임을 정지한다 
public class OnMouseDown_StopGame : MonoBehaviour
{

	void Start ()// 처음에 시행한다
	{ 
		Time.timeScale = 1; // 시간을 움직인다 
	}

	void OnMouseDown()// 터치하면 
	{ 
		Time.timeScale = 0; // 시간을 멈춘다
	}
}
