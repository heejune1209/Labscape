using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� ���ӽ����̽�

public class LobbyManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // ������ �̸��� ������ ��ȯ
        SceneManager.LoadScene(sceneName);
    }
}
