using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환에 필요 

// 카운터가 최종값이면 씬을 전환한다 
public class OnCountFinished_SwitchScene : MonoBehaviour
{

    public int lastCount = 3; // 카운터의 최종값 : Inspector에 지정
    public string sceneName = ""; // 씬 이름：Inspector에 지정

    void FixedUpdate() // 계속 시행한다
    { 
        // 카운터가 최종값이 되면 
        if (GameCounter.value == lastCount)
        {
            // 씬을 전환한다 
            SceneManager.LoadScene (sceneName);
        }
    }
}
