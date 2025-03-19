using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchieveUI : MonoBehaviour
{
    private Animator animator;
    public Image ShortClearprogressBar; // 진행도 바의 이미지
    public Image DeathprogressBar; // 진행도 바의 이미지
    public Image TrapprogressBar; // 진행도 바의 이미지
    public Image StageClearprogressBar; // 진행도 바의 이미지
    public float ShortClearfillAmount;
    public float DeathfillAmount; // 진행도 값을 나타내는 변수 (0f에서 1f 사이)
    public float TrapfillAmount;
    public float StageClearfillAmount;

    // 업데이트 메서드에서 진행도 바를 업데이트합니다.
    void Update()
    {
        // 진행도 바의 fillAmount를 업데이트합니다.
        ShortClearprogressBar.fillAmount = ShortClearfillAmount;
        DeathprogressBar.fillAmount = DeathfillAmount;
        TrapprogressBar.fillAmount = TrapfillAmount;
        StageClearprogressBar.fillAmount = StageClearfillAmount;

    }

    public void ShortClearSetProgress(float progress)
    {
        ShortClearfillAmount = Mathf.Clamp01(progress); // 진행도를 0에서 1 사이로 제한합니다.
    }
    // 외부에서 진행도 바의 값을 설정할 수 있는 메서드입니다.
    public void DeathSetProgress(float progress)
    {
        DeathfillAmount = Mathf.Clamp01(progress); // 진행도를 0에서 1 사이로 제한합니다.
    }
    public void TrapSetProgress(float progress)
    {
        TrapfillAmount = Mathf.Clamp01(progress); // 진행도를 0에서 1 사이로 제한합니다.
    }
    public void StageClearSetProgress(float progress)
    {
        StageClearfillAmount = Mathf.Clamp01(progress); // 진행도를 0에서 1 사이로 제한합니다.
    }
    public void CloseAchieveUI()    // 버튼으로 로직 연결, UI닫는 메소드 호출 기능
    {
        animator = GetComponent<Animator>();
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UI창 닫는 애니메이션 실행과 0.5초 텀 추가
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
}
