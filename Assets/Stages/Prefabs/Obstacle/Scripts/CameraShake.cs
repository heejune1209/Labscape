using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 카메라 흔들림 효과를 실행하는 코루틴
    public IEnumerator Shake(float duration, float magnitude)
    {
        // 원래 카메라 로컬 위치 저장
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        // 지정한 duration 동안 반복
        while (elapsed < duration)
        {
            // x, y축에 랜덤 값을 곱해 흔들림 효과 생성
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // 원래 위치에 랜덤 오프셋을 더한 위치로 설정
            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 흔들림 종료 후 원래 위치로 복원
        transform.localPosition = originalPosition;
    }
}
