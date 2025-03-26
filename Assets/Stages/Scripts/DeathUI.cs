using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    public TMP_Text deathText; // UI �ؽ�Ʈ
    public float duration = 2.0f; // �ִϸ��̼� ���� �ð�

    

    public void OnPlayerDeath()
    {
        gameObject.SetActive(true);
        // �÷��̾ �׾��� �� ȣ��Ǵ� �޼ҵ�
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

        deathText.transform.localScale = originalScale; // ���� ũ��� �ǵ�����
        gameObject.SetActive(false); // ��� �г� ��Ȱ��ȭ
    }
}
