using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 카운터를 증가한다 
public class OnMouseDown_Count : MonoBehaviour
{

	public int addValue = 1;	// 증가량 : Inspector에 지정

	void OnMouseDown()// 터치하며
	{ 
		// 카운터 값을 증가한다 
		GameCounter.value = GameCounter.value + addValue;
	}
}
