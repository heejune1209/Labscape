using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�


[RequireComponent(typeof(TextMeshProUGUI))] // TextMeshProUGUI ������Ʈ�� �ʿ�
public class BlinkText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public float duration = 3.0f; // ���İ��� ����Ǵ� �� �ɸ��� �ð�

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
        // ���İ��� 0���� ����
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
        // ���İ��� 1�� ����
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            yield return null;
        }

        StartCoroutine(FadeTextToZeroAlpha()); // �ٽ� ���İ��� 0���� �����ϴ� �ڷ�ƾ�� ����
    }
}
