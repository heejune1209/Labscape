using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGround : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;
    public float fallingSpeed = 5.0f;
    public float destroyDelay = 5f; // ���� �������� ���� �ı��Ǳ� �������� �ð�
    public List<GameObject> obstacles; // �迭�� ����� ��ֹ� ������Ʈ
    public float obstacleFallDelay = 3f; // ��ֹ� ������Ʈ�� �������� ���� �ð�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // ó������ �������� �ʵ��� ����

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                Rigidbody2D obstacleRb = obstacle.GetComponent<Rigidbody2D>();
                if (obstacleRb != null)
                {
                    obstacleRb.isKinematic = true; // ��ֹ��� ó������ �������� �ʵ��� ����
                    obstacleRb.gravityScale = 0; // �߷� �������� 0���� ����
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            StartCoroutine(FallAndDestroy());
        }
    }

    IEnumerator FallAndDestroy()
    {
        rb.isKinematic = false; // ���� ���������� ����
        rb.velocity = Vector2.down * 5f; // ���� �������� �ӵ� ���� (�ʿ信 ���� ���� ����)
        yield return new WaitForSeconds(obstacleFallDelay); // ��ֹ� ������Ʈ�� �������� ���� �ð� ���

        FallingObjects(); // ��ֹ� ����߸���
        yield return new WaitForSeconds(destroyDelay); // ������ �ı� ���� �ð���ŭ ���

        Destroy(gameObject); // ������Ʈ ����     
    }

    private void FallingObjects()
    {
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                Rigidbody2D obstacleRb = obstacle.GetComponent<Rigidbody2D>();
                if (obstacleRb != null)
                {
                    obstacleRb.isKinematic = false; // ��ֹ��� ���������� ����
                    obstacleRb.gravityScale = 1f; // �߷� �������� �ٽ� 1�� ����
                    obstacleRb.velocity = Vector2.down * fallingSpeed; // ��ֹ��� �������� �ӵ� ���� (�ʿ信 ���� ���� ����)
                    StartCoroutine(DestroyObstacleAfterDelay(obstacleRb.gameObject, 3f)); // 3�� �� ��ֹ� �ı�
                }
            }
        }
    }

    IEnumerator DestroyObstacleAfterDelay(GameObject obstacle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obstacle);
    }
}