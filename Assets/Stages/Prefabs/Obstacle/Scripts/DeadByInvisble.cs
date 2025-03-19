using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadByInvisble : MonoBehaviour
{    
    public GameObject trap;
    public bool trapInitiallyActive = false; // 에디터에서 설정할 수 있는 초기 상태 변수
    private void Start()
    {
        trap.SetActive(trapInitiallyActive);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                trap.SetActive(true);
                player.Die();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            trap.SetActive(true);
        }
    }
}
