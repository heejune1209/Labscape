using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 표시한다 
public class OnCollision_Show : MonoBehaviour
{

    public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
    public string showObjectName;   // 표시 오브젝트 이름 : Inspector에 지정

    GameObject showObject;

    void Start()// 처음에 시행한다 
    { 
        // 지우기 전에 표시 오브젝트를 기억해 둔다 
        showObject = GameObject.Find(showObjectName);
        showObject.SetActive(false); // 지운다
    }

    void OnCollisionEnter2D(Collision2D collision)// 충돌했을 때
    { 
        // 만약 충돌한 것의 이르이 목표 오브젝트면 
        if (collision.gameObject.name == targetObjectName)
        {
            showObject.SetActive(true); // 지웠던 것을 표시한다 
        }
    }
}
