using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadbyContact : MonoBehaviour
{
    private Animator animator;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {                
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f; // �߷� ��Ȱ��ȭ
                animator.SetTrigger("Ishit"); // �ִϸ��̼� �Ķ���� ����
                player.Die();
            }
        }
    }
    
}
