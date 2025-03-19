using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환에 필요

// 충돌하면 씬을 전환한다
public class OnCollision_SwitchScene : MonoBehaviour
{

    public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
    public string sceneName;  // 씬 이름 : Inspector에 지정

    void OnCollisionEnter2D(Collision2D collision)// 충돌했을 때
    { 
      // 만약 충돌한 것의 이름이 목표 오브젝트였다면
        if (collision.gameObject.name == targetObjectName)
        {
            // 씬을 전환한다
            SceneManager.LoadScene(sceneName);
        }
    }
}
