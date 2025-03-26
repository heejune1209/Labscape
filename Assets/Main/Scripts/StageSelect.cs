using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    private Animator animator;

    public void CloseSelectUI()    // ��ư���� ���� ����, UI�ݴ� �޼ҵ� ȣ�� ���
    {
        animator = GetComponent<Animator>();
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UIâ �ݴ� �ִϸ��̼� ����� 0.5�� �� �߰�
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
}
