using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 게임을 정지한다
public class OnCollision_StopGame : MonoBehaviour
{

    public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정

    void Start()
    { // 처음에 시행한다
      // 시간을 움직인다
        Time.timeScale = 1;
    }

    void OnCollisionEnter2D(Collision2D collision)// 충돌했을 때
    { 
      // 만약 충돌한 것의 이름이 목표 오브젝트였다면
        if (collision.gameObject.name == targetObjectName)
        {
            // 시간을 멈춘다
            Time.timeScale = 0;
        }
    }
}
