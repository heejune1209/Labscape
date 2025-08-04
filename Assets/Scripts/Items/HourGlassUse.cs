using System.Collections;
using UnityEngine;

namespace Labscape.Items
{
    /// <summary>
    /// 모래시계 아이템 사용 시 플레이어를 무적 상태로 만들고 이동을 정지시킵니다.
    /// PlayerController의 ActivateHourGlass API를 사용합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "HourGlassUse", menuName = "Game/ItemUse/HourGlassUse", order = 100)]
    public class HourGlassUse : ScriptableObject, IItemUse
    {
        [Header("모래시계 이펙트 Prefab")]
        [Tooltip("플레이어 위치에 소환될 모래시계 프리팹")]
        public GameObject hourglassEffectPrefab;

        [Header("Item Data (buffDuration 포함)")]
        [Tooltip("지속시간 등 메타데이터를 가진 SO를 할당")]
        public ItemData data;

        // IItemUse 인터페이스 구현: 아이템 사용 시 호출됩니다.
        public void Use(GameObject user)
        {
            if (user == null)
            {
                Debug.LogWarning("HourGlassUse: user가 null입니다.");
                return;
            }

            // PlayerController 컴포넌트 찾기
            var playerController = user.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("HourGlassUse: PlayerController 컴포넌트를 찾을 수 없습니다.");
                return;
            }

            // 지속시간을 SO에서 가져옴
            float duration = data.buffDuration;
            if (duration <= 0f)
            {
                Debug.LogWarning($"HourGlassUse: data.buffDuration이 0 이하입니다. ({duration})");
                return;
            }

            // PlayerController를 통한 모래시계 효과 활성화
            playerController.ActivateHourGlass();

            // 시각적 이펙트 생성
            GameObject effectInstance = null;
            if (hourglassEffectPrefab != null)
            {
                effectInstance = Managers.Resource.Instantiate(
                    $"Prefabs/Items/{hourglassEffectPrefab.name}",
                    user.transform.position,
                    Quaternion.identity, null, data._initialpoolcount
                );
                
                if (effectInstance != null)
                {
                    effectInstance.transform.SetParent(user.transform);
                    
                    // 모래시계 활성화 사운드 재생
                    Managers.Sound.PlaySFX(5); // 모래시계 전용 사운드 인덱스
                }
                else
                {
                    Debug.LogWarning($"HourGlassUse: 이펙트 인스턴스화에 실패했습니다: {hourglassEffectPrefab.name}");
                }
            }

            // MonoBehaviour 코루틴 실행용 (이펙트 제거)
            var mb = user.GetComponent<MonoBehaviour>();
            if (mb != null && effectInstance != null)
            {
                mb.StartCoroutine(RemoveEffectAfterDelay(effectInstance, duration));
            }
            else if (effectInstance != null)
            {
                Debug.LogWarning("HourGlassUse: MonoBehaviour를 찾을 수 없어 이펙트 제거 코루틴을 시작하지 못했습니다.");
            }

            Debug.Log($"HourGlassUse: 모래시계 효과 활성화 - 지속시간: {duration}초");
        }

        /// <summary>
        /// 지정된 시간 후 이펙트를 제거하는 코루틴
        /// PlayerController의 모래시계 효과는 자체적으로 타이머 관리
        /// </summary>
        /// <param name="effect">제거할 이펙트 오브젝트</param>
        /// <param name="duration">지연 시간</param>
        /// <returns></returns>
        private IEnumerator RemoveEffectAfterDelay(GameObject effect, float duration)
        {
            yield return new WaitForSeconds(duration);
            
            if (effect != null)
            {
                Destroy(effect);
            }
        }
    }
}