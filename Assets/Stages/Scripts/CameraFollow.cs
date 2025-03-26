using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 카메라가 따라갈 대상 (플레이어 등)
    public float smoothing = 0.3f;  // 카메라 이동의 부드러움 정도 (SmoothDamp 시간)
    public Vector2 minCameraPos; // 카메라 이동 최소 위치 제한
    public Vector2 maxCameraPos; // 카메라 이동 최대 위치 제한
    public float threshold;  // 대상이 이동해야 카메라가 따라가기 시작하는 임계값

    public float upperYBoundary;  // 대상이 상단을 벗어났을 때 기준 y값
    public float lowerYBoundary;  // 대상이 하단을 벗어났을 때 기준 y값
    public float UpverticalOffset;  // 대상이 상단을 벗어났을 때 카메라에 적용할 y 오프셋
    public float DownverticalOffset;  // 대상이 하단을 벗어났을 때 카메라에 적용할 y 오프셋

    private Vector3 velocity = Vector3.zero;  // SmoothDamp에 사용되는 속도 변수
    private float originalY;  // 초기 카메라 y 위치

    void Start()
    {
        // 시작 시 현재 카메라 y 위치 저장
        originalY = transform.position.y;
    }

    void FixedUpdate()
    {
        // 대상의 위치를 기준으로 카메라 목표 위치 계산
        Vector3 targetCamPos = target.position;

        // x축 처리: 대상의 x 위치가 threshold보다 크면 대상 위치 사용, 아니면 현재 보정
        if (target.position.x > threshold)
        {
            targetCamPos.x = target.position.x;
        }
        else
        {
            targetCamPos.x += transform.position.x - target.position.x;
        }

        // y축 처리: 대상 위치에 따라 카메라 y 위치 조정
        if (target.position.y >= upperYBoundary)
        {
            // 대상이 상단을 넘으면 UpverticalOffset 적용
            targetCamPos.y = target.position.y + UpverticalOffset;
        }
        else if (target.position.y <= lowerYBoundary)
        {
            // 대상이 하단 아래이면 DownverticalOffset 적용
            targetCamPos.y = target.position.y - DownverticalOffset;
        }
        else
        {
            // 그 외에는 카메라 y 위치를 원래값과 부드럽게 보간
            targetCamPos.y = Mathf.Lerp(transform.position.y, originalY, smoothing);
        }

        // z축: 카메라 z 위치는 유지
        targetCamPos.z = transform.position.z;

        // x, y 위치를 최소/최대 값으로 제한
        targetCamPos.x = Mathf.Clamp(targetCamPos.x, minCameraPos.x, maxCameraPos.x);
        targetCamPos.y = Mathf.Clamp(targetCamPos.y, minCameraPos.y, maxCameraPos.y);

        // SmoothDamp를 사용하여 카메라 위치를 부드럽게 목표 위치로 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetCamPos, ref velocity, smoothing);
    }
}
