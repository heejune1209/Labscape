using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrigger : MonoBehaviour
{
    public GameObject _rocket; // 발사할 파이어볼 프리팹
    public GameObject ShootPoint;
    public float speed = 5f; // 파이어볼의 속도
    public Vector2 direction = Vector2.up; // 발사 방향을 에디터에서 설정할 수 있도록 필드 추가  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            LaunchFireball(direction);
        }
    }
    public void LaunchFireball(Vector2 direction)
    {
        // 발사 지점에서 파이어볼 인스턴스 생성
        GameObject _fireballPrefab = Instantiate(_rocket, ShootPoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = _fireballPrefab.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed; // 방향 벡터를 정규화하고 속도를 곱하여 속도 설정
    }
}
