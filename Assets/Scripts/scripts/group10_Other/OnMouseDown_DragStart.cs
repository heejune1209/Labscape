using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 드래그한다 
public class OnMouseDown_DragStart: MonoBehaviour
{

	bool dragFlag = false;

	void OnMouseDown()
	{
		dragFlag = true;
	}
	void OnMouseUp()
	{
		dragFlag = false;
	}
	void Update()
	{ // 계속 시행한다 
		if (dragFlag)
		{
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = -5f; // 앞 쪽에 표시
            this.gameObject.transform.position = position;
		}
	}
}
