using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속, 이동한다(수직)
public class Forever_MoveV : MonoBehaviour
{

    public float speed = 1; // 속도：Inspector에 지정

    void FixedUpdate()
    { // 계속 시행한다（일정 시간마다）
        this.transform.Translate(0 ,speed/50, 0); // 수직 이동한다
    }
}

