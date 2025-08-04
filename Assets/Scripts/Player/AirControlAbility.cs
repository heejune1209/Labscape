using System.Collections;
using UnityEngine;

/// <summary>
/// 공중 제어 능력 - WingUse에서 점프 부스트용으로 사용
/// PlayerController와 연동하여 점프력 조정
/// </summary>
public class AirControlAbility : MonoBehaviour
{
    [Header("점프 부스트 설정")]
    [SerializeField] private float originalJumpForce = 6.0f;
    [SerializeField] private float currentJumpMultiplier = 1.0f;

    // PlayerController 참조
    private PlayerController playerController;

    // 부스트 상태 추적
    private bool isBoostActive = false;
    private Coroutine boostCoroutine;

    void Awake()
    {
        // PlayerController 참조 획득
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[AirControlAbility] PlayerController 컴포넌트를 찾을 수 없습니다");
        }
        else
        {
            // PlayerController의 초기 점프력 저장
            originalJumpForce = playerController.jumpForce;
        }
    }

    /// <summary>
    /// 점프 부스트 효과 적용
    /// </summary>
    /// <param name="multiplier">점프력 배수</param>  
    /// <param name="duration">지속 시간</param>
    public void UseJumpBoost(float multiplier, float duration)
    {
        if (playerController == null)
        {
            Debug.LogWarning("[AirControlAbility] PlayerController가 null입니다");
            return;
        }

        // 기존 부스트가 활성화되어 있으면 중지
        if (isBoostActive && boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
        }

        // 새로운 부스트 시작
        boostCoroutine = StartCoroutine(ApplyJumpBoost(multiplier, duration));
    }

    /// <summary>
    /// 점프 부스트 효과 코루틴
    /// </summary>
    /// <param name="multiplier">점프력 배수</param>
    /// <param name="duration">지속 시간</param>
    /// <returns></returns>
    private IEnumerator ApplyJumpBoost(float multiplier, float duration)
    {
        isBoostActive = true;
        currentJumpMultiplier = multiplier;

        // 원래 점프력 저장
        float originalJump = playerController.jumpForce;

        // 점프력 증가 적용
        playerController.jumpForce = originalJump * multiplier;

        Debug.Log($"[AirControlAbility] 점프 부스트 활성화 - 배수: {multiplier}x, 지속시간: {duration}초");

        // 지속 시간 대기
        yield return new WaitForSeconds(duration);

        // 원래 점프력으로 복원
        if (playerController != null)
        {
            playerController.jumpForce = originalJump;
        }

        isBoostActive = false;
        currentJumpMultiplier = 1.0f;

        Debug.Log("[AirControlAbility] 점프 부스트 효과 종료");
    }

    /// <summary>
    /// 현재 부스트 상태 확인
    /// </summary>
    /// <returns>부스트 활성화 여부</returns>
    public bool IsBoostActive()
    {
        return isBoostActive;
    }

    /// <summary>
    /// 현재 점프 배수 반환
    /// </summary>
    /// <returns>현재 점프 배수</returns>
    public float GetCurrentMultiplier()
    {
        return currentJumpMultiplier;
    }

    /// <summary>
    /// 부스트 효과 강제 해제
    /// </summary>
    public void CancelBoost()
    {
        if (isBoostActive && boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            
            // 원래 점프력으로 복원
            if (playerController != null)
            {
                playerController.jumpForce = originalJumpForce;
            }
            
            isBoostActive = false;
            currentJumpMultiplier = 1.0f;
            
            Debug.Log("[AirControlAbility] 점프 부스트 강제 해제");
        }
    }

    void OnDestroy()
    {
        // 코루틴 정리
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
        }
    }
}