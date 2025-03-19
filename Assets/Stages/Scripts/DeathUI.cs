using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    public TMP_Text deathText; // UI 텍스트
    public float duration = 2.0f; // 애니메이션 지속 시간

    

    public void OnPlayerDeath()
    {
        gameObject.SetActive(true);
        // 플레이어가 죽었을 때 호출되는 메소드
        StartCoroutine(AnimateDeathText());
    }

    private IEnumerator AnimateDeathText()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = deathText.transform.localScale;

        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(1f, 1.5f, Mathf.PingPong(elapsedTime, duration / 2));
            deathText.transform.localScale = originalScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        deathText.transform.localScale = originalScale; // 원래 크기로 되돌리기
        gameObject.SetActive(false); // 사망 패널 비활성화
    }
}
