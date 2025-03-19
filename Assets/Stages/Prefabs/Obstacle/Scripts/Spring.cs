using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f; // �÷��̾ ���� �������� ��
    public GameObject spring;
    public bool springInitiallyActive = false; // �����Ϳ��� ������ �� �ִ� �ʱ� ���� ����
    //public float ReenableTime = 4f;
    //public Button targetButton; // ��Ȱ��ȭ�� ��ư ������Ʈ

    private void Start()
    {
        spring.SetActive(springInitiallyActive);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            Collider2D playerCollider = collision.GetComponent<Collider2D>();;
            if (playerRb != null && playerCollider != null)
            {
                // ���ö�� ������ �÷��̾ ���� ��������
                spring.SetActive(true);
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Elevator"), true);
                // �÷��̾��� ��� �浹�� �Ͻ������� ����
                //playerCollider.enabled = false;
                //targetButton.interactable = false; // ��ư ��Ȱ��ȭ
                // ���� �ð� �Ŀ� �÷��̾��� �浹�� �ٽ� Ȱ��ȭ
                //StartCoroutine(ReenableCollision(playerCollider, ReenableTime)); // 1.5�� �Ŀ� �浹 �ٽ� Ȱ��ȭ
            }
        }
    }
    /*
    IEnumerator ReenableCollision(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        //collider.enabled = true;
        //targetButton.interactable = true; // ��ư �ٽ� Ȱ��ȭ
    }
    */
}
