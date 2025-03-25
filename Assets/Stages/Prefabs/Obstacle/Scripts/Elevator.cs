using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{    
    public List<GameObject> Doors;
    public GameObject elevator;      // Elevator object that will move up
    public float duration = 7f;      // Duration for the elevator to move before disappearing
    void Start()
    {
        // ���� ���� �� ��� ���� BoxCollider2D�� ��Ȱ��ȭ
        foreach (GameObject door in Doors)
        {
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false; // BoxCollider2D ��Ȱ��ȭ
                door.SetActive(false); // �� ��ü�� ��Ȱ��ȭ
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ targettrigger�� ����� �� �˻�
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("TargetTrigger"))
        {
            StartCoroutine(ActivateDoorsAfterDelay(0.5f)); // 1�� �Ŀ� ���� Ȱ��ȭ
        }
    }

    IEnumerator ActivateDoorsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(��) ���� ���
        foreach (GameObject door in Doors)
        {
            door.SetActive(true); // �� �� Ȱ��ȭ
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true; // BoxCollider2D Ȱ��ȭ

            }
        }
        // Wait another 0.5 seconds before moving the elevator
        yield return new WaitForSeconds(0.5f);

        // Start moving the elevator up
        StartCoroutine(MoveElevatorUp());
    }
    IEnumerator MoveElevatorUp()
    {
        float moveSpeed = 2f;  // Speed at which the elevator moves up
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            elevator.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), true);
        }

        // Optionally make the elevator disappear or deactivate after moving up
        elevator.SetActive(false);
    }

}
