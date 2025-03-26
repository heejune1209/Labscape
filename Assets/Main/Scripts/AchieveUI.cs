using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchieveUI : MonoBehaviour
{
    private Animator animator;
    public Image ShortClearprogressBar; // ���൵ ���� �̹���
    public Image DeathprogressBar; // ���൵ ���� �̹���
    public Image TrapprogressBar; // ���൵ ���� �̹���
    public Image StageClearprogressBar; // ���൵ ���� �̹���
    public float ShortClearfillAmount;
    public float DeathfillAmount; // ���൵ ���� ��Ÿ���� ���� (0f���� 1f ����)
    public float TrapfillAmount;
    public float StageClearfillAmount;

    // ������Ʈ �޼��忡�� ���൵ �ٸ� ������Ʈ�մϴ�.
    void Update()
    {
        // ���൵ ���� fillAmount�� ������Ʈ�մϴ�.
        ShortClearprogressBar.fillAmount = ShortClearfillAmount;
        DeathprogressBar.fillAmount = DeathfillAmount;
        TrapprogressBar.fillAmount = TrapfillAmount;
        StageClearprogressBar.fillAmount = StageClearfillAmount;

    }

    public void ShortClearSetProgress(float progress)
    {
        ShortClearfillAmount = Mathf.Clamp01(progress); // ���൵�� 0���� 1 ���̷� �����մϴ�.
    }
    // �ܺο��� ���൵ ���� ���� ������ �� �ִ� �޼����Դϴ�.
    public void DeathSetProgress(float progress)
    {
        DeathfillAmount = Mathf.Clamp01(progress); // ���൵�� 0���� 1 ���̷� �����մϴ�.
    }
    public void TrapSetProgress(float progress)
    {
        TrapfillAmount = Mathf.Clamp01(progress); // ���൵�� 0���� 1 ���̷� �����մϴ�.
    }
    public void StageClearSetProgress(float progress)
    {
        StageClearfillAmount = Mathf.Clamp01(progress); // ���൵�� 0���� 1 ���̷� �����մϴ�.
    }
    public void CloseAchieveUI()    // ��ư���� ���� ����, UI�ݴ� �޼ҵ� ȣ�� ���
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
