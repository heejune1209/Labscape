using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float toggleInterval = 1.0f; // �������� ������ ������ ����
    private SpriteRenderer laserSprite; // �������� ��������Ʈ ������
    private BoxCollider2D laserCollider; // �������� �ݶ��̴�

    private bool isLaserActive = false; // �������� Ȱ�� ����

    private void Awake()
    {
        laserCollider = GetComponent<BoxCollider2D>();
        laserSprite = GetComponent<SpriteRenderer>();
        // �������� �ʱ� ���·� ����
        laserSprite.enabled = false;
        laserCollider.enabled = false;
    }
    private void Start()
    {
        
        // �������� ������ ������ �ݺ��ϴ� �ڷ�ƾ ����
        StartCoroutine(ToggleLaser());
    }

    private IEnumerator ToggleLaser()
    {
        while (true)
        {
            // ���� �ð��� ����
            float startTime = Time.time;
            // ������ ���� ��ȯ
            isLaserActive = !isLaserActive;
            laserSprite.enabled = isLaserActive;
            laserCollider.enabled = isLaserActive;

            // ���� ��۱��� ���
            while (Time.time < startTime + toggleInterval)
            {
                yield return null;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������� Ȱ��ȭ�� ���¿��� �÷��̾�� �浹���� ��
        if (isLaserActive && collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Die();
        }
    }
}
