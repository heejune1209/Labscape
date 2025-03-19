using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 계속 카운터 값을 표시한다
public class Forever_ShowCount : MonoBehaviour
{

    void Update()// 계속
    { 
      // 카운터 값을 표시한다 
        GetComponent<Text>().text = GameCounter.value.ToString();
    }
}
