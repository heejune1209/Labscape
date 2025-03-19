using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시간이 다 되면 자기 자신을 삭제한디
public class OnTimeout_DestroyMe : MonoBehaviour
{

    public float limitSec = 3; // 초 수 : Inspector에 지정

    void Start()
    { // 처음에 시행한다
        Destroy(this.gameObject, limitSec); // 지정 초 후에 소멸하는 예약 
    }
}
