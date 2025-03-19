using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 계속, 이동한다(수평)
public class Forever_MoveH : MonoBehaviour
{

    public float speed = 1; // 속도 : Inspector에 지정

    void FixedUpdate()  // 계속 시행한다(일정 시간마다)
    { 
        this.transform.Translate(speed/50, 0, 0); // 수평 이동한다
    }
}
