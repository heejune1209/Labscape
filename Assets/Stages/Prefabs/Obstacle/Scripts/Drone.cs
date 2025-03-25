using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed = 2.0f;  // ����� �̵� �ӵ�
    public bool moveRight = true;  // �ʱ� �̵� ���� ���� (true�� ������, false�� ����)
    public float detectionRadius = 5.0f;  // �÷��̾ �����ϴ� �ݰ�

    private Transform player;
    private bool isChasing = false;
    [HideInInspector]
    public bool isSpawned = false;  // ����� �����Ǿ����� ����

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
        // �¿�� �̵�
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
        // ���� �ݰ� ���� �÷��̾ �ִ��� ����
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        // �÷��̾ �߰�
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isSpawned)
        {
            // �÷��̾�� �浹 �� �÷��̾ ����
            player.GetComponent<PlayerController>().Die();
        }
        else if (collision.CompareTag("Ground") && isChasing)
        {
            // ���� �߿� Ground �±׸� ���� ������Ʈ�� �浹 �� ��� ����
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // �����Ϳ��� ���� �ݰ��� �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
