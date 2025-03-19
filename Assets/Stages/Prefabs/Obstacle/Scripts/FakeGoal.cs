using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGoal : MonoBehaviour
{
    public float RightDisplacement = 0.5f; // 플레이어를 부드럽게 이동시킬 x 좌표
    public float hideDuration = 1.0f;       // 플레이어가 숨겨지는 시간
    public float launchDistance = -10f;          // 플레이어를 x축 반대 방향으로 이동시킬 거리
    public float launchDuration = 0.5f;         // 플레이어가 부드럽게 이동하는 시간
    public GameObject player;               // 플레이어 오브젝트
    public Transform launchPoint;               // 플레이어를 날려보낼 위치 오브젝트
    public GameObject Boom;
    public GameObject targetObject; // 태그를 변경할 오브젝트
    public string newTag; // 새 태그

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FakeGoalRoutine(collision.gameObject));
            AudioManager.instance.PlaySFX(10);
            ChangeTag();
        }
    }
    private void ChangeTag()
    {
        if (targetObject != null && !string.IsNullOrEmpty(newTag))
        {
            targetObject.tag = newTag;
            
        }       
    }

    private IEnumerator FakeGoalRoutine(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // 부드럽게 X축으로 이동
            Vector3 targetPosition = player.transform.position + new Vector3(RightDisplacement, 0, 0);
            float elapsedTime = 0;

            while (elapsedTime < RightDisplacement)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, (elapsedTime / RightDisplacement));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 플레이어를 숨김
            player.SetActive(false);
            yield return new WaitForSeconds(hideDuration);

            // 플레이어를 다시 활성화하고 날려보냄
            player.transform.position = launchPoint.position;
            player.SetActive(true);
            Boom.SetActive(true);
            yield return null; // 한 프레임 대기하여 활성화가 완료되도록 함

            // 1.5초 후에 Boom 오브젝트 비활성화
            StartCoroutine(DeactivateBoomAfterDelay(1.5f));

            // 부드럽게 launchDistance만큼 이동
            Vector3 startPosition = player.transform.position;
            Vector3 endPosition = new Vector3(startPosition.x + launchDistance, startPosition.y, startPosition.z);
            elapsedTime = 0;

            while (elapsedTime < launchDuration)
            {
                player.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / launchDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 최종 위치 보정
            player.transform.position = endPosition;
        }
    }
    private IEnumerator DeactivateBoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Boom.SetActive(false);
    }


}
