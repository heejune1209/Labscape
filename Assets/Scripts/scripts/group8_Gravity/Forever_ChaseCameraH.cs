using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 계속 카메라가 쫓아간다(수평으로) 
public class Forever_ChaseCameraH : MonoBehaviour
{

    Vector3 base_pos;

    void Start() // 처음에 시행한
    { 
        // 카메라의 원래 위치를 기억해 둔다 
        base_pos = Camera.main.gameObject.transform.position;
    }

    void LateUpdate()// 계속 시행한다(여러 가지 처리의 마지막에)
    { 
        Vector3 pos = this.transform.position; // 자신의 위치 
        pos.z = -10; // 카메라이므로 앞으로 이동시킨다 
        pos.y = base_pos.y; // 카메라 원래의 높이를 사용한다 
        Camera.main.gameObject.transform.position = pos;
    }
}