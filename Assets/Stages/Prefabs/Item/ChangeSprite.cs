using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public List<string> targetLayers; // 장애물 오브젝트의 레이어 리스트
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void Start()
    {
        // 레이어 리스트에 있는 레이어를 가진 모든 오브젝트를 찾아 원래 색깔을 저장
        foreach (string layerName in targetLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                Debug.LogWarning($"Layer '{layerName}' does not exist.");
                continue;
            }

            GameObject[] objects = FindObjectsOfType<GameObject>(true); // 비활성화된 오브젝트 포함
            foreach (GameObject obj in objects)
            {
                if (obj.layer == layer)
                {
                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        // 원래 색깔 저장
                        if (!originalColors.ContainsKey(obj))
                        {
                            originalColors[obj] = spriteRenderer.color;
                        }
                    }
                }
            }
        }
    }

    // 탐지기 아이템을 사용했을 때 호출되는 메소드
    public void ActivateDetector()
    {
        StartCoroutine(ChangeSpriteColorRoutine(6.0f)); // 6초 동안 스프라이트 색깔 변경
    }

    private IEnumerator ChangeSpriteColorRoutine(float duration)
    {
        bool hasInvisibleLayer = targetLayers.Count > 2;
        int invisibleLayer = hasInvisibleLayer ? LayerMask.NameToLayer(targetLayers[2]) : -1;

        // 레이어 리스트에 있는 레이어를 가진 모든 오브젝트의 스프라이트 색깔을 빨간색으로 변경하고 필요한 경우 비활성화된 오브젝트를 활성화
        foreach (string layerName in targetLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            GameObject[] objects = FindObjectsOfType<GameObject>(true); // 비활성화된 오브젝트 포함
            foreach (GameObject obj in objects)
            {
                if (obj.layer == layer)
                {
                    if (layer == invisibleLayer)
                    {
                        obj.SetActive(true); // 비활성화된 오브젝트를 활성화
                    }

                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.red;
                    }
                }
            }
        }

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(duration);

        // 스프라이트 색깔을 원래 색깔로 복원하고 필요한 경우 다시 비활성화
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
                    entry.Key.SetActive(false); // 비활성화
                }
            }
        }
    }
}