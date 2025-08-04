using System;
using UnityEngine;

/// <summary>
/// 플레이어 체력 시스템 - DeathZone에서 즉사 처리용으로 사용
/// PlayerController와 연동하여 사망 처리
/// </summary>
public class Health : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP = 100;

    // PlayerController 참조
    private PlayerController playerController;

    // 체력 변경 이벤트
    public event Action<int, int> OnHealthChanged; // (currentHP, maxHP)
    public event Action OnDeath;

    public int CurrentHP 
    { 
        get => currentHP; 
        private set 
        { 
            int oldHP = currentHP;
            currentHP = Mathf.Clamp(value, 0, maxHP);
            
            if (oldHP != currentHP)
            {
                OnHealthChanged?.Invoke(currentHP, maxHP);
                
                // 체력이 0이 되면 사망 처리
                if (currentHP <= 0 && oldHP > 0)
                {
                    Die();
                }
            }
        } 
    }

    public int MaxHP 
    { 
        get => maxHP; 
        set 
        { 
            maxHP = Mathf.Max(1, value);
            CurrentHP = Mathf.Min(CurrentHP, maxHP); // 현재 HP가 최대치를 넘지 않도록
        }
    }

    public bool IsDead => CurrentHP <= 0;

    void Awake()
    {
        // PlayerController 참조 획득
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[Health] PlayerController 컴포넌트를 찾을 수 없습니다");
        }

        // 초기 체력 설정
        CurrentHP = maxHP;
    }

    /// <summary>
    /// 데미지를 입습니다
    /// </summary>
    /// <param name="damage">받을 데미지</param>
    public void Damage(int damage)
    {
        if (IsDead)
        {
            Debug.LogWarning("[Health] 이미 사망한 상태에서 추가 데미지를 받았습니다");
            return;
        }

        if (damage <= 0)
        {
            Debug.LogWarning($"[Health] 잘못된 데미지 값: {damage}");
            return;
        }

        CurrentHP -= damage;
        Debug.Log($"[Health] {damage} 데미지를 받았습니다. 현재 HP: {CurrentHP}/{maxHP}");
    }

    /// <summary>
    /// 체력을 회복합니다
    /// </summary>
    /// <param name="amount">회복량</param>
    public void Heal(int amount)
    {
        if (IsDead)
        {
            Debug.LogWarning("[Health] 사망한 상태에서는 회복할 수 없습니다");
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning($"[Health] 잘못된 회복량: {amount}");
            return;
        }

        CurrentHP += amount;
        Debug.Log($"[Health] {amount} 체력을 회복했습니다. 현재 HP: {CurrentHP}/{maxHP}");
    }

    /// <summary>
    /// 체력을 최대치로 완전 회복합니다
    /// </summary>
    public void FullHeal()
    {
        CurrentHP = maxHP;
        Debug.Log($"[Health] 체력을 완전 회복했습니다. HP: {CurrentHP}/{maxHP}");
    }

    /// <summary>
    /// 즉사 처리
    /// </summary>
    public void InstantKill()
    {
        CurrentHP = 0;
        Debug.Log("[Health] 즉사 처리됨");
    }

    /// <summary>
    /// 사망 처리 - PlayerController의 Die() 메서드 호출
    /// </summary>
    private void Die()
    {
        Debug.Log("[Health] 플레이어 사망 처리 시작");

        // PlayerController를 통한 사망 처리
        if (playerController != null)
        {
            playerController.Die();
        }
        else
        {
            Debug.LogError("[Health] PlayerController가 null이어서 사망 처리를 할 수 없습니다");
        }

        // 사망 이벤트 발생
        OnDeath?.Invoke();
    }

    /// <summary>
    /// 리스폰 시 체력 초기화
    /// </summary>
    public void ResetOnRespawn()
    {
        CurrentHP = maxHP;
        Debug.Log("[Health] 리스폰으로 인한 체력 초기화");
    }

    /// <summary>
    /// 체력 비율 반환 (0.0 ~ 1.0)
    /// </summary>
    /// <returns>체력 비율</returns>
    public float GetHealthRatio()
    {
        return maxHP > 0 ? (float)CurrentHP / maxHP : 0f;
    }

    void OnDestroy()
    {
        // 이벤트 해제
        OnHealthChanged = null;
        OnDeath = null;
    }
}