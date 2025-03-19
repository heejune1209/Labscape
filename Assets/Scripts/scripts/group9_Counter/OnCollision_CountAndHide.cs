using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 카운터를 증가하고 자신을 지운다 
public class OnCollision_CountAndHide : MonoBehaviour
{
    
    public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
    public int addValue = 1;    // 증가량：Inspector에 지정

    void OnCollisionEnter2D(Collision2D collision) // 충돌했을 때
    { 
        // 만약 충돌한 것의 이름이 목표 오브젝트면 
        if (collision.gameObject.name == targetObjectName)
        {
            // 카운터 값을 증가한다 
            GameCounter.value = GameCounter.value + addValue;
            // 자기자신을 지운다 
            this.gameObject.SetActive(false);
        }
    }
}
