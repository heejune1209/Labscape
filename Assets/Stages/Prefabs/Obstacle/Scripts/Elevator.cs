using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public List<GameObject> Doors; // 엘리베이터 문 오브젝트 리스트
    public GameObject elevator;    // 움직일 엘리베이터 오브젝트
    public float duration = 7f;    // 엘리베이터가 이동하는 시간 (지속 시간)

    void Start()
    {
        // 시작 시 모든 문의 BoxCollider2D를 비활성화하고 문 오브젝트를 숨김
        foreach (GameObject door in Doors)
        {
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false; // 충돌 감지 비활성화
                door.SetActive(false);    // 문 오브젝트 비활성화
            }
        }
    }

    // 플레이어가 엘리베이터 트리거 영역을 벗어나면 호출
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 충돌한 오브젝트가 "Player"이고, 현재 오브젝트의 태그가 "TargetTrigger"이면
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("TargetTrigger"))
        {
            // 0.5초 후 문을 활성화하고 엘리베이터 이동을 시작하는 코루틴 실행
            StartCoroutine(ActivateDoorsAfterDelay(0.5f));
        }
    }

    // 지정한 지연 시간 후 모든 문을 활성화하고 엘리베이터를 위로 이동시키는 코루틴
    IEnumerator ActivateDoorsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지연 시간 대기

        foreach (GameObject door in Doors)
        {
            door.SetActive(true); // 문 오브젝트 활성화
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true; // Collider 활성화
            }
        }
        // 추가로 0.5초 대기 후 엘리베이터 이동 코루틴 실행
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveElevatorUp());
    }

    // 엘리베이터를 위로 이동시키는 코루틴
    IEnumerator MoveElevatorUp()
    {
        float moveSpeed = 2f;  // 엘리베이터 이동 속도
        float startTime = Time.time; // 이동 시작 시간 저장

        // duration 동안 매 프레임마다 엘리베이터를 위로 이동
        while (Time.time - startTime < duration)
        {
            elevator.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
            // 이동 중 플레이어와 "Block" 레이어 간 충돌 무시
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), true);
        }

        // 이동이 완료되면 엘리베이터 오브젝트를 비활성화 (사라짐)
        elevator.SetActive(false);
    }
}
