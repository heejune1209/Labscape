using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public List<string> targetLayers; // ��ֹ� ������Ʈ�� ���̾� ����Ʈ
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void Start()
    {
        // ���̾� ����Ʈ�� �ִ� ���̾ ���� ��� ������Ʈ�� ã�� ���� ������ ����
        foreach (string layerName in targetLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                Debug.LogWarning($"Layer '{layerName}' does not exist.");
                continue;
            }

            GameObject[] objects = FindObjectsOfType<GameObject>(true); // ��Ȱ��ȭ�� ������Ʈ ����
            foreach (GameObject obj in objects)
            {
                if (obj.layer == layer)
                {
                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        // ���� ���� ����
                        if (!originalColors.ContainsKey(obj))
                        {
                            originalColors[obj] = spriteRenderer.color;
                        }
                    }
                }
            }
        }
    }

    // Ž���� �������� ������� �� ȣ��Ǵ� �޼ҵ�
    public void ActivateDetector()
    {
        StartCoroutine(ChangeSpriteColorRoutine(6.0f)); // 6�� ���� ��������Ʈ ���� ����
    }

    private IEnumerator ChangeSpriteColorRoutine(float duration)
    {
        bool hasInvisibleLayer = targetLayers.Count > 2;
        int invisibleLayer = hasInvisibleLayer ? LayerMask.NameToLayer(targetLayers[2]) : -1;

        // ���̾� ����Ʈ�� �ִ� ���̾ ���� ��� ������Ʈ�� ��������Ʈ ������ ���������� �����ϰ� �ʿ��� ��� ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ
        foreach (string layerName in targetLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            GameObject[] objects = FindObjectsOfType<GameObject>(true); // ��Ȱ��ȭ�� ������Ʈ ����
            foreach (GameObject obj in objects)
            {
                if (obj.layer == layer)
                {
                    if (layer == invisibleLayer)
                    {
                        obj.SetActive(true); // ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ
                    }

                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.red;
                    }
                }
            }
        }

        // ������ �ð� ���� ���
        yield return new WaitForSeconds(duration);

        // ��������Ʈ ������ ���� ����� �����ϰ� �ʿ��� ��� �ٽ� ��Ȱ��ȭ
        foreach (KeyValuePair<GameObject, Color> entry in originalColors)
        {
            if (entry.Key != null)
            {
                SpriteRenderer spriteRenderer = entry.Key.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = entry.Value;
                }
                if (entry.Key.layer == invisibleLayer)
                {
                    entry.Key.SetActive(false); // ��Ȱ��ȭ
                }
            }
        }
    }
}