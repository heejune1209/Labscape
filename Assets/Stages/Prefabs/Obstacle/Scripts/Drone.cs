using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed = 2.0f;  // 드론의 이동 속도
    public bool moveRight = true;  // 초기 이동 방향 설정 (true면 오른쪽, false면 왼쪽)
    public float detectionRadius = 5.0f;  // 플레이어를 감지하는 반경

    private Transform player;
    private bool isChasing = false;
    [HideInInspector]
    public bool isSpawned = false;  // 드론이 스폰되었는지 여부

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isSpawned)
        {
            if (isChasing)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
                DetectPlayer();
            }
        }
    }

    void Patrol()
    {
        // 좌우로 이동
        if (moveRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void DetectPlayer()
    {
        // 일정 반경 내에 플레이어가 있는지 감지
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        // 플레이어를 추격
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isSpawned)
        {
            // 플레이어와 충돌 시 플레이어를 죽임
            player.GetComponent<PlayerController>().Die();
        }
        else if (collision.CompareTag("Ground") && isChasing)
        {
            // 추적 중에 Ground 태그를 가진 오브젝트와 충돌 시 드론 제거
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // 에디터에서 감지 반경을 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
