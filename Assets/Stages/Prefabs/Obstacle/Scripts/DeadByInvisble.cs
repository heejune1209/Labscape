using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadByInvisble : MonoBehaviour
{    
    public GameObject trap;
    public bool trapInitiallyActive = false; // �����Ϳ��� ������ �� �ִ� �ʱ� ���� ����
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
