using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면 밖으로 나가면 자기자신을 삭제한다 
public class OnOutsideScene_DestroyMe : MonoBehaviour
{

	bool showFlag = false;

	void LateUpdate()// 계속 시행한다(일정 시간마다) 
	{ 
		if (GetComponent<Renderer>().isVisible)
		{
			// 1회 표시된 다음 
			showFlag = true;
		} else {
			if (showFlag)
			{
				// 화면 밖으로 나와서 사라지면 삭제한다
				Destroy(this.gameObject);
			}
		}
	}
}
