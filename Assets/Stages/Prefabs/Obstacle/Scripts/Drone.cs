using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed = 2.0f;  // 드론 이동 속도
    public bool moveRight = true;  // 드론이 기본적으로 오른쪽으로 이동할지 여부 (true: 오른쪽, false: 왼쪽)
    public float detectionRadius = 5.0f;  // 플레이어 탐지 반경

    private Transform player; // 플레이어의 Transform 참조
    private bool isChasing = false; // 플레이어 추적 여부
    [HideInInspector]
    public bool isSpawned = false;  // 드론이 생성되었는지 여부

    void Start()
    {
        // "Player" 태그를 가진 오브젝트의 Transform 참조
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isSpawned)
        {
            if (isChasing)
            {
                // 플레이어를 추적하는 함수 호출
                ChasePlayer();
            }
            else
            {
                // 기본 순찰 이동 실행
                Patrol();
                // 플레이어 탐지 함수 호출
                DetectPlayer();
            }
        }
    }

    // 기본 순찰: moveRight 값에 따라 좌우 이동
    void Patrol()
    {
        if (moveRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    // 플레이어와의 거리 계산을 통해 탐지 상태 전환
    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true;
        }
    }

    // 플레이어를 향해 이동하는 추적 함수
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // 2D 트리거 충돌 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isSpawned)
        {
            // 플레이어와 충돌 시 PlayerController의 Die() 호출
            player.GetComponent<PlayerController>().Die();
        }
        else if (collision.CompareTag("Ground") && isChasing)
        {
            // 플레이어 추적 중 땅에 부딪히면 드론 삭제
            Destroy(gameObject);
        }
    }

    // 에디터에서 탐지 반경을 시각적으로 표시하는 함수
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
