using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환에 필요 

// 터치하면 씬을 전환한다 
public class OnMouseDown_SwitchScene : MonoBehaviour
{

    public string sceneName;  // 씬 이름：Inspector에 지정

    void OnMouseDown() 
	{ // 터치하면
        // 씬을 전환한다 
        SceneManager.LoadScene (sceneName);
    }
}
