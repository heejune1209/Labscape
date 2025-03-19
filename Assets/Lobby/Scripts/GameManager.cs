using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public int PlayerDeathCount { get; set; }
    public int spannerCount { get; set; }  // �÷��̾ ���� ���г� ��

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("GameManager");
                    _instance = gameObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    public DateTime StartTime { get; set; }

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            StartTime = DateTime.UtcNow;
        }
        else
        {
            Destroy(gameObject);

        }
    }

    public void GoToLobby()
    {
        GameManager.Instance.StartTime = DateTime.UtcNow;
        GameManager.Instance.PlayerDeathCount = 0;
        GameManager.Instance.spannerCount = 0;
    }
    public void PlayerDie() // �׾�����
    {
        GameManager.Instance.PlayerDeathCount++;
        string sceneName = SceneManager.GetActiveScene().name;
        int stageNumber;
        if (sceneName.Contains("Stage") && int.TryParse(sceneName.Replace("Stage", ""), out stageNumber))
        {
            PlayerPrefs.SetInt("Death" + stageNumber, GameManager.Instance.PlayerDeathCount);
        }

    }
    public void AddSpanner()
    {
        GameManager.Instance.spannerCount++;
        PlayerPrefs.SetInt("Gem", GameManager.Instance.spannerCount);  // ���г� �� ����
        PlayerPrefs.Save();  // ���� ���� ����
    }

    public void LoadScene(string sceneName)
    {
        // ������ �̸��� ������ ��ȯ
        SceneManager.LoadScene(sceneName);
    }

    public void ClosePanelUI(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnclickQuit()
    {
#if UNITY_EDITOR       
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
