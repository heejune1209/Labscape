using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    public List<GameObject> Doors;
    public float Closetime = 0.5f;
    // Start is called before the first frame update
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
            StartCoroutine(ActivateDoorsAfterDelay1(Closetime)); // 1�� �Ŀ� ���� Ȱ��ȭ
        }
    }
    IEnumerator ActivateDoorsAfterDelay1(float delay)
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
    }

}
