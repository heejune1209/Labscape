using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 룰렛처럼 회전한다
public class OnMouseDown_Roulette : MonoBehaviour
{

    public float maxSpeed = 50; // 최대 속도 : Inspector에 지정

    float rotateAngle = 0;

    void OnMouseDown()
    { // 터치하면
        rotateAngle = maxSpeed; // 최대 속도를 낸다
    }
    void FixedUpdate()
    { // 계속 시행한다(일정 시간마다)
        rotateAngle = rotateAngle * (float)0.98; // 회전량을 조금씩 줄여서
        this.transform.Rotate(0, 0, rotateAngle); // 회전한다
    }
}
