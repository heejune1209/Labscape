using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public int PlayerDeathCount { get; set; }
    public int spannerCount { get; set; }  // 플레이어가 가진 스패너 수

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
    public void PlayerDie() // 죽었을떄
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
        PlayerPrefs.SetInt("Gem", GameManager.Instance.spannerCount);  // 스패너 수 저장
        PlayerPrefs.Save();  // 변경 사항 적용
    }

    public void LoadScene(string sceneName)
    {
        // 지정된 이름의 씬으로 전환
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
