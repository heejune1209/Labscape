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
        originalColor = spriteRenderer.color; // ���� ���� ����
        

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
        StartCoroutine(ChangeSpriteColor(6.0f)); // 6�� ���� ��������Ʈ ���� ����
    }

    IEnumerator ChangeSpriteColor(float duration)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor; // ���� ����� ����

    }
}
