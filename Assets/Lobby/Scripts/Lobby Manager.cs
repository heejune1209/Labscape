using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위한 네임스페이스

public class LobbyManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // 지정된 이름의 씬으로 전환
        SceneManager.LoadScene(sceneName);
    }
}
