using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    private Animator animator;

    public void CloseSelectUI()    // 버튼으로 로직 연결, UI닫는 메소드 호출 기능
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
