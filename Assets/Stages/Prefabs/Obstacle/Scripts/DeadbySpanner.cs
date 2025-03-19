using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadbySpanner : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 원래 색깔 저장
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();           
            if (player != null)
            {
                ChangeSprite();
                player.Die();
            }
        }
    }

    
    private void ChangeSprite()
    {
        StartCoroutine(ChangeSpriteColor(6.0f)); // 6초 동안 스프라이트 색깔 변경
    }

    IEnumerator ChangeSpriteColor(float duration)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor; // 원래 색깔로 복원

    }
}
