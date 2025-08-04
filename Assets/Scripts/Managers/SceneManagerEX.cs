using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Labscape.Manager
{
    public class SceneManagerEX
{
    // 싱글톤을 통해 다른 매니저들에 접근

    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    // GetSceneName는 Define.Scene enum을 string으로 변환하는 함수
    // 즉, 이 함수는 씬의 이름을 가져오는 역할을 한다.
    public string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type); // C#의 Reflection. Scene enum의 
        return name;
    }

    /// <summary>
    /// SceneManagerEX 초기화
    /// </summary>
    public void Init()
    {
        Debug.Log("SceneManagerEX: Initialize");
    }

    public void LoadScene(Define.Scene type)
    {
        // 1) 씬 UI만 지우기
        Managers.UI?.ClearSceneUI();

        // 2) 팝업만 지우기 + 풀 등 전역 정리
        Managers.UI?.ClearPopupUI();
        Managers.Pool.Clear();

        SceneManager.LoadScene(GetSceneName(type)); // SceneManager는 UnityEngine의 SceneManager
    }
    
    /// <summary>
    /// SceneManagerEX 정리
    /// </summary>
    public void Clear()
    {
        Debug.Log("SceneManagerEX: Clear");
        CurrentScene?.Clear();
    }
}
}