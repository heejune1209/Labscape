using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 애니메이션을 전환한다
public class OnMouseDown_ChangeAnime : MonoBehaviour
{

	public string normalAnime = ""; // 보통 애니메이션：Inspector에 지정
	public string nextAnime = "";   // 다음 애니메이션：Inspector에 지정

	bool touchFlag = false;

	void Start()// 처음은 아무 것도 하지 않는다
	{ 
	}

	void OnMouseDown()// 터치하면
	{ 
		touchFlag = !touchFlag;
		Animator animator = this.GetComponent<Animator>();
		if (touchFlag == true)
		{ // 다음 애니메이션으로 전환한다
			animator.Play(nextAnime);
		}
		else
		{ // 한 번 더 터치하면 원래의 애니메이션으로 전환한다 
			animator.Play(normalAnime);
		}
	}
}
