using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌하면 표시한다(여러 개 대응)
public class OnMultiCollision_Show : MonoBehaviour
{

    public string targetObjectName; // 목표 오브젝트 이름 : Inspector에 지정
    public string showObjectName;  // 표시 오브젝트 이름 : Inspector에 지정

    GameObject showObject;
    float orgY = 0;
    float ofsetY = 10000; // 이 값을 Y 방향에 더하는 것으로 지운다

    void Start()
    { // 처음에 시행한다
      // 지우기 전에 표시 오브젝트를 기억해둔다
        showObject = GameObject.Find(showObjectName);

        // 지운다 【showObject.SetActive(false); 대신】
        orgY = showObject.transform.position.y;
        if (orgY > ofsetY)
        {
            orgY -= ofsetY;
        }
        Vector3 pos = showObject.transform.position;
        pos.y = orgY + ofsetY;
        showObject.transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    { // 충돌했을 때
      // 만약 충돌한 것의 이름이 목표 오브젝트였다면
        if (collision.gameObject.name == targetObjectName)
        {
            // 표시하는 【showObject.SetActive(true); 대신】
            Vector3 pos = showObject.transform.position;
            pos.y = orgY;
            showObject.transform.position = pos;
        }
    }
}
