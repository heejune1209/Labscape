using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    public TMP_Text loadingText; // 로딩 중 표시할 텍스트 UI 요소

    void Start()
    {
        // PlayerPrefs에 저장된 "nextScene" 값을 읽어와 다음에 로드할 씬 이름을 가져옴
        string nextScene = PlayerPrefs.GetString("nextScene");
        // 비동기로 씬 로드를 시작하는 코루틴 실행
        StartCoroutine(LoadSceneAsync(nextScene));

        /*
        // 예시: 다음 씬 이름을 설정하고 로딩 씬으로 전환하는 코드 (주석 처리됨)
        PlayerPrefs.SetString("nextScene", "다음 씬 이름");
        SceneManager.LoadScene("LoadingScene");
        */
    }

    // 씬을 비동기 로드하는 코루틴
    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 씬을 비동기로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        // 로드 완료 후 자동 활성화를 막음
        operation.allowSceneActivation = false;

        // 로딩 애니메이션 코루틴 실행 (텍스트 애니메이션)
        StartCoroutine(LoadingAnimation());

        while (!operation.isDone)
        {
            // 씬 로드 진행도가 0.9 이상이면 (실제로 완료 상태는 0.9까지)
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);  // 추가 대기 (3초)

                // 대기 후 씬 활성화
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    // 로딩 텍스트에 애니메이션 효과를 주는 코루틴
    IEnumerator LoadingAnimation()
    {
        while (true)
        {
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.3f);
            loadingText.text = "Loading....";
            yield return new WaitForSeconds(0.3f);
            loadingText.text = "Loading.....";
            yield return new WaitForSeconds(0.3f);
            loadingText.text = "Loading....";
            yield return new WaitForSeconds(0.3f);
        }
    }
}
