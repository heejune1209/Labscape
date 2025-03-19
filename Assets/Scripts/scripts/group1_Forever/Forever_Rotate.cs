using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속, 회전한다
public class Forever_Rotate : MonoBehaviour
{

    public float angle = 90; // 각도：Inspector에 지정

    void FixedUpdate() // 계속 시행한다(일정 시간마다)
    { 
        this.transform.Rotate(0, 0, angle/50);  // 회전한다
    }
}
