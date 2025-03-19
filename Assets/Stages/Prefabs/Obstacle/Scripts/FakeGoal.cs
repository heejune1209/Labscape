using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGoal : MonoBehaviour
{
    public float RightDisplacement = 0.5f; // �÷��̾ �ε巴�� �̵���ų x ��ǥ
    public float hideDuration = 1.0f;       // �÷��̾ �������� �ð�
    public float launchDistance = -10f;          // �÷��̾ x�� �ݴ� �������� �̵���ų �Ÿ�
    public float launchDuration = 0.5f;         // �÷��̾ �ε巴�� �̵��ϴ� �ð�
    public GameObject player;               // �÷��̾� ������Ʈ
    public Transform launchPoint;               // �÷��̾ �������� ��ġ ������Ʈ
    public GameObject Boom;
    public GameObject targetObject; // �±׸� ������ ������Ʈ
    public string newTag; // �� �±�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FakeGoalRoutine(collision.gameObject));
            AudioManager.instance.PlaySFX(10);
            ChangeTag();
        }
    }
    private void ChangeTag()
    {
        if (targetObject != null && !string.IsNullOrEmpty(newTag))
        {
            targetObject.tag = newTag;
            
        }       
    }

    private IEnumerator FakeGoalRoutine(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // �ε巴�� X������ �̵�
            Vector3 targetPosition = player.transform.position + new Vector3(RightDisplacement, 0, 0);
            float elapsedTime = 0;

            while (elapsedTime < RightDisplacement)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, (elapsedTime / RightDisplacement));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // �÷��̾ ����
            player.SetActive(false);
            yield return new WaitForSeconds(hideDuration);

            // �÷��̾ �ٽ� Ȱ��ȭ�ϰ� ��������
            player.transform.position = launchPoint.position;
            player.SetActive(true);
            Boom.SetActive(true);
            yield return null; // �� ������ ����Ͽ� Ȱ��ȭ�� �Ϸ�ǵ��� ��

            // 1.5�� �Ŀ� Boom ������Ʈ ��Ȱ��ȭ
            StartCoroutine(DeactivateBoomAfterDelay(1.5f));

            // �ε巴�� launchDistance��ŭ �̵�
            Vector3 startPosition = player.transform.position;
            Vector3 endPosition = new Vector3(startPosition.x + launchDistance, startPosition.y, startPosition.z);
            elapsedTime = 0;

            while (elapsedTime < launchDuration)
            {
                player.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / launchDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ���� ��ġ ����
            player.transform.position = endPosition;
        }
    }
    private IEnumerator DeactivateBoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Boom.SetActive(false);
    }


}
