using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 네임스페이스 추가


[RequireComponent(typeof(TextMeshProUGUI))] // TextMeshProUGUI 컴포넌트가 필요
public class BlinkText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public float duration = 3.0f; // 알파값이 변경되는 데 걸리는 시간

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(FadeTextToZeroAlpha());
    }

    IEnumerator FadeTextToZeroAlpha()
    {
        // 알파값이 0으로 감소
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            yield return null;
        }

        StartCoroutine(FadeTextToOneAlpha());
    }

    IEnumerator FadeTextToOneAlpha()
    {
        // 알파값이 1로 증가
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            yield return null;
        }

        StartCoroutine(FadeTextToZeroAlpha()); // 다시 알파값이 0으로 감소하는 코루틴을 시작
    }
}
