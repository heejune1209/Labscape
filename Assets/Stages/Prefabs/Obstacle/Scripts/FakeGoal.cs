using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGoal : MonoBehaviour
{
    public float RightDisplacement = 0.5f; // 플레이어를 오른쪽으로 이동시킬 거리
    public float hideDuration = 1.0f;       // 플레이어를 잠시 숨길 시간
    public float launchDistance = -10f;     // 플레이어를 발사할 때 x축으로 이동할 거리 (음수이면 왼쪽)
    public float launchDuration = 0.5f;     // 발사 이동에 걸리는 시간
    public GameObject player;               // 플레이어 오브젝트 참조
    public Transform launchPoint;           // 플레이어가 발사 후 이동할 위치
    public GameObject Boom;                 // 발사 후 폭발 효과 오브젝트
    public GameObject targetObject;         // 태그를 변경할 대상 오브젝트
    public string newTag;                   // 새 태그 문자열

    // 플레이어와 충돌하면 FakeGoal 루틴 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FakeGoalRoutine(collision.gameObject));
            AudioManager.instance.PlaySFX(10);
            ChangeTag();
        }
    }

    // targetObject의 태그를 newTag로 변경
    private void ChangeTag()
    {
        if (targetObject != null && !string.IsNullOrEmpty(newTag))
        {
            targetObject.tag = newTag;
        }
    }

    // 플레이어의 FakeGoal 동작을 처리하는 코루틴
    private IEnumerator FakeGoalRoutine(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // 플레이어를 오른쪽으로 약간 이동시킬 목표 위치 계산
            Vector3 targetPosition = player.transform.position + new Vector3(RightDisplacement, 0, 0);
            float elapsedTime = 0;

            // 일정 시간동안 Lerp로 플레이어 위치를 서서히 이동
            while (elapsedTime < RightDisplacement)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, (elapsedTime / RightDisplacement));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 플레이어를 숨김
            player.SetActive(false);
            yield return new WaitForSeconds(hideDuration);

            // 플레이어를 launchPoint 위치로 이동시키고 다시 활성화
            player.transform.position = launchPoint.position;
            player.SetActive(true);
            Boom.SetActive(true);
            yield return null; // 폭발 효과 시작 후 잠깐 대기

            // 1.5초 후 Boom 효과 비활성화 코루틴 시작
            StartCoroutine(DeactivateBoomAfterDelay(1.5f));

            // 플레이어를 발사할 목표 위치 계산 (현재 위치에서 launchDistance만큼 이동)
            Vector3 startPosition = player.transform.position;
            Vector3 endPosition = new Vector3(startPosition.x + launchDistance, startPosition.y, startPosition.z);
            elapsedTime = 0;

            // launchDuration 동안 Lerp로 플레이어를 발사
            while (elapsedTime < launchDuration)
            {
                player.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / launchDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 최종 위치를 설정
            player.transform.position = endPosition;
        }
    }

    // 일정 시간 후 Boom 효과를 비활성화하는 코루틴
    private IEnumerator DeactivateBoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Boom.SetActive(false);
    }
}
