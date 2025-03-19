using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGround : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;
    public float fallingSpeed = 5.0f;
    public float destroyDelay = 5f; // 땅이 떨어지고 나서 파괴되기 전까지의 시간
    public List<GameObject> obstacles; // 배열로 선언된 장애물 오브젝트
    public float obstacleFallDelay = 3f; // 장애물 오브젝트가 떨어지는 지연 시간

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // 처음에는 움직이지 않도록 설정

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                Rigidbody2D obstacleRb = obstacle.GetComponent<Rigidbody2D>();
                if (obstacleRb != null)
                {
                    obstacleRb.isKinematic = true; // 장애물도 처음에는 움직이지 않도록 설정
                    obstacleRb.gravityScale = 0; // 중력 스케일을 0으로 설정
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
        rb.isKinematic = false; // 땅이 떨어지도록 설정
        rb.velocity = Vector2.down * 5f; // 땅이 떨어지는 속도 설정 (필요에 따라 조정 가능)
        yield return new WaitForSeconds(obstacleFallDelay); // 장애물 오브젝트가 떨어지는 지연 시간 대기

        FallingObjects(); // 장애물 떨어뜨리기
        yield return new WaitForSeconds(destroyDelay); // 설정된 파괴 지연 시간만큼 대기

        Destroy(gameObject); // 오브젝트 삭제     
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
                    obstacleRb.isKinematic = false; // 장애물이 떨어지도록 설정
                    obstacleRb.gravityScale = 1f; // 중력 스케일을 다시 1로 설정
                    obstacleRb.velocity = Vector2.down * fallingSpeed; // 장애물의 떨어지는 속도 설정 (필요에 따라 조정 가능)
                    StartCoroutine(DestroyObstacleAfterDelay(obstacleRb.gameObject, 3f)); // 3초 후 장애물 파괴
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