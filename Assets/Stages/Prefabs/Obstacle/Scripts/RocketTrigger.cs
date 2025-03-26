using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTrigger : MonoBehaviour
{
    public GameObject _rocket; // �߻��� ���̾ ������
    public GameObject ShootPoint;
    public float speed = 5f; // ���̾�� �ӵ�
    public Vector2 direction = Vector2.up; // �߻� ������ �����Ϳ��� ������ �� �ֵ��� �ʵ� �߰�  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            LaunchFireball(direction);
        }
    }
    public void LaunchFireball(Vector2 direction)
    {
        // �߻� �������� ���̾ �ν��Ͻ� ����
        GameObject _fireballPrefab = Instantiate(_rocket, ShootPoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = _fireballPrefab.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed; // ���� ���͸� ����ȭ�ϰ� �ӵ��� ���Ͽ� �ӵ� ����
    }
}
