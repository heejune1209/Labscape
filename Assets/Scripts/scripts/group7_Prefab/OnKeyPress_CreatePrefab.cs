using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키를 누르면 근처에 프리팹을 만든다 
public class OnKeyPress_CreatePrefab : MonoBehaviour
{

	public GameObject newPrefab; // 만드는 프리팹：Inspector에 지정
	public string inkey = "space";
	public float offsetX = 1;
	public float offsetY = 1;

	bool pushFlag = false;

	void Update ()// 계속 시행한다 
	{ 
		if(Input.GetKey(inkey))// 만약 키가 눌리면 
		{ 
			if (pushFlag == false)
			{
				pushFlag = true;
				Vector3 area = this.GetComponent<SpriteRenderer>().bounds.size;
				Vector3 newPos = this.transform.position;

				newPos.x += offsetX;
				newPos.y += offsetY;
				newPos.z = -5; // 앞 쪽에 표시 
				// 프리팹을 만든다 
				GameObject newGameObject = Instantiate(newPrefab) as GameObject;
				newGameObject.transform.position = newPos;
			}
		} else
		{
			pushFlag = false;
		}
	}

}
