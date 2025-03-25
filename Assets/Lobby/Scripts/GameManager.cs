using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 전체의 상태와 데이터를 관리하는 싱글톤 GameManager 클래스
public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장하는 변수
    private static GameManager _instance;

    // 플레이어의 사망 횟수를 저장하는 프로퍼티
    public int PlayerDeathCount { get; set; }
    // 플레이어가 획득한 스패너(아이템)의 개수를 저장하는 프로퍼티
    public int spannerCount { get; set; }  // 플레이어가 획득한 스패너 개수

    // 싱글톤 인스턴스에 전역 접근할 수 있는 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 기존 인스턴스가 없으면 씬 내에서 찾아보고, 없으면 새로 생성
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

    // 게임 시작 시간을 저장하는 프로퍼티
    public DateTime StartTime { get; set; }

    // Awake: 오브젝트가 생성될 때 호출되며, 싱글톤 인스턴스를 초기화하고 중복 생성을 방지함
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // 씬이 변경되어도 삭제되지 않도록 설정
            DontDestroyOnLoad(gameObject);
            // 게임 시작 시간을 현재 시간으로 설정
            StartTime = DateTime.UtcNow;
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 오브젝트를 삭제
            Destroy(gameObject);
        }
    }

    // 로비로 이동할 때 게임 관련 데이터를 초기화하는 메서드
    public void GoToLobby()
    {
        // 시작 시간을 현재 시간으로 재설정
        GameManager.Instance.StartTime = DateTime.UtcNow;
        // 플레이어 사망 횟수를 초기화
        GameManager.Instance.PlayerDeathCount = 0;
        // 스패너 개수를 초기화
        GameManager.Instance.spannerCount = 0;
    }

    // 플레이어 사망 시 호출되어 사망 횟수를 증가시키고, 해당 스테이지의 사망 횟수를 저장하는 메서드
    public void PlayerDie() // 플레이어 사망 처리
    {
        GameManager.Instance.PlayerDeathCount++;
        // 현재 활성화된 씬의 이름을 가져옴
        string sceneName = SceneManager.GetActiveScene().name;
        int stageNumber;
        // 씬 이름이 "Stage"를 포함하고 뒤에 붙은 숫자를 스테이지 번호로 파싱
        if (sceneName.Contains("Stage") && int.TryParse(sceneName.Replace("Stage", ""), out stageNumber))
        {
            // 해당 스테이지의 사망 횟수를 PlayerPrefs에 저장
            PlayerPrefs.SetInt("Death" + stageNumber, GameManager.Instance.PlayerDeathCount);
        }
    }

    // 스패너 개수를 증가시키고 저장하는 메서드
    public void AddSpanner()
    {
        GameManager.Instance.spannerCount++;
        // 스패너(또는 젬) 개수를 PlayerPrefs에 저장
        PlayerPrefs.SetInt("Gem", GameManager.Instance.spannerCount);
        PlayerPrefs.Save();  // 변경사항을 저장
    }

    // 지정된 씬 이름으로 씬 전환을 처리하는 메서드
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 전달받은 UI 패널 오브젝트를 비활성화하여 닫는 메서드
    public void ClosePanelUI(GameObject obj)
    {
        obj.SetActive(false);
    }

    // 모든 PlayerPrefs 데이터를 초기화하는 메서드 (데이터 리셋)
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    // 게임 종료 혹은 에디터 플레이 모드 종료를 처리하는 메서드
    public void OnclickQuit()
    {
#if UNITY_EDITOR       
        // 에디터 모드에서 실행 중일 경우, 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 어플리케이션 종료
        Application.Quit();
#endif
    }
}