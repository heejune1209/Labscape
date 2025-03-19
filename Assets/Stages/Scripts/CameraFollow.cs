using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 카메라가 따라갈 대상, 보통 플레이어
    public float smoothing = 0.3f;  // 카메라 움직임의 부드러움 정도(낮을수록 더 부드러움)
    public Vector2 minCameraPos; // 카메라의 최소 위치
    public Vector2 maxCameraPos; // 카메라의 최대 위치
    public float threshold;  // 중앙에 위치하기 시작하는 임계점

    public float upperYBoundary;  // 상단 y 경계값
    public float lowerYBoundary;  // 하단 y 경계값
    public float UpverticalOffset;  // 위로의 y축 오프셋(카메라가 상으로 이동할 때 적용할 y축 오프셋) 플레이어 위치에서 올라갈 정도
    public float DownverticalOffset;  // 아래의 y축 오프셋(카메라가 하로 이동할 때 적용할 y축 오프셋) 플레이어 위치에서 내려갈 정도

    private Vector3 velocity = Vector3.zero;  // 내부 속도 참조 (SmoothDamp 사용)
    private float originalY;  // 초기 y 위치 저장

    void Start()
    {
        originalY = transform.position.y;  // 게임 시작 시 카메라의 초기 y 위치 저장
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position;

        if (target.position.x > threshold)
        {
            targetCamPos.x = target.position.x;
        }
        else
        {
            targetCamPos.x += transform.position.x - target.position.x;
        }

        // 플레이어의 y 위치에 따라 카메라의 y 위치 조정
        if (target.position.y >= upperYBoundary)
        {
            targetCamPos.y = target.position.y + UpverticalOffset;  // 상단 경계에 도달하면 카메라 위치를 플레이어 포지션에서 살짝 위로
        }
        else if (target.position.y <= lowerYBoundary)
        {
            targetCamPos.y = target.position.y - DownverticalOffset;  // 하단 경계에 도달하면 카메라 위치를 플레이어 포지션에서 살짝 아래로
        }
        else
        {
            // 플레이어가 상하 경계 사이에 있을 경우 원래의 y 위치로 부드럽게 돌아감
            targetCamPos.y = Mathf.Lerp(transform.position.y, originalY, smoothing);
        }

        targetCamPos.z = transform.position.z;  // z축은 변경하지 않음

        targetCamPos.x = Mathf.Clamp(targetCamPos.x, minCameraPos.x, maxCameraPos.x);
        targetCamPos.y = Mathf.Clamp(targetCamPos.y, minCameraPos.y, maxCameraPos.y);

        transform.position = Vector3.SmoothDamp(transform.position, targetCamPos, ref velocity, smoothing);
    }
}
