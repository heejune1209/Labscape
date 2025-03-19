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
                rb.gravityScale = 0f; // 중력 비활성화
                animator.SetTrigger("Ishit"); // 애니메이션 파라미터 설정
                player.Die();
            }
        }
    }
    
}
