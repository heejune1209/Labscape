using UnityEngine;

/// <summary>
/// 플레이어 이동 시스템 - FeatherUse에서 속도 조정용으로 사용
/// PlayerController와 연동하여 작동
/// </summary>
public class Locomotion : MonoBehaviour
{
    [Header("이동 속도 설정")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.0f;

    // PlayerController 참조
    private PlayerController playerController;

    public float WalkSpeed 
    { 
        get => walkSpeed; 
        set 
        { 
            walkSpeed = value;
            // PlayerController의 speed도 함께 업데이트
            if (playerController != null)
            {
                playerController.speed = walkSpeed;
            }
        } 
    }

    public float SprintSpeed 
    { 
        get => sprintSpeed; 
        set 
        { 
            sprintSpeed = value;
            // 필요시 PlayerController의 달리기 속도도 설정
            if (playerController != null)
            {
                // PlayerController에 sprintSpeed가 없으므로 일반 speed로 적용
                playerController.speed = sprintSpeed;
            }
        } 
    }

    void Awake()
    {
        // PlayerController 참조 획득
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[Locomotion] PlayerController 컴포넌트를 찾을 수 없습니다");
        }
        else
        {
            // PlayerController의 초기 속도를 walkSpeed로 설정
            playerController.speed = walkSpeed;
        }
    }

    /// <summary>
    /// 현재 이동 속도 반환 (PlayerController 기준)
    /// </summary>
    /// <returns>현재 속도</returns>
    public float GetCurrentSpeed()
    {
        return playerController != null ? playerController.speed : walkSpeed;
    }

    /// <summary>
    /// 속도를 원래 값으로 복원
    /// </summary>
    public void ResetSpeed()
    {
        WalkSpeed = 3.0f; // 기본값으로 복원
        SprintSpeed = 5.0f;
    }
}