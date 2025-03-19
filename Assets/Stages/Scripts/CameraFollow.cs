using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // ī�޶� ���� ���, ���� �÷��̾�
    public float smoothing = 0.3f;  // ī�޶� �������� �ε巯�� ����(�������� �� �ε巯��)
    public Vector2 minCameraPos; // ī�޶��� �ּ� ��ġ
    public Vector2 maxCameraPos; // ī�޶��� �ִ� ��ġ
    public float threshold;  // �߾ӿ� ��ġ�ϱ� �����ϴ� �Ӱ���

    public float upperYBoundary;  // ��� y ��谪
    public float lowerYBoundary;  // �ϴ� y ��谪
    public float UpverticalOffset;  // ������ y�� ������(ī�޶� ������ �̵��� �� ������ y�� ������) �÷��̾� ��ġ���� �ö� ����
    public float DownverticalOffset;  // �Ʒ��� y�� ������(ī�޶� �Ϸ� �̵��� �� ������ y�� ������) �÷��̾� ��ġ���� ������ ����

    private Vector3 velocity = Vector3.zero;  // ���� �ӵ� ���� (SmoothDamp ���)
    private float originalY;  // �ʱ� y ��ġ ����

    void Start()
    {
        originalY = transform.position.y;  // ���� ���� �� ī�޶��� �ʱ� y ��ġ ����
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position;

        if (target.position.x > threshold)
        {
            targetCamPos.x = target.position.x;
        }
        else
        {
            targetCamPos.x += transform.position.x - target.position.x;
        }

        // �÷��̾��� y ��ġ�� ���� ī�޶��� y ��ġ ����
        if (target.position.y >= upperYBoundary)
        {
            targetCamPos.y = target.position.y + UpverticalOffset;  // ��� ��迡 �����ϸ� ī�޶� ��ġ�� �÷��̾� �����ǿ��� ��¦ ����
        }
        else if (target.position.y <= lowerYBoundary)
        {
            targetCamPos.y = target.position.y - DownverticalOffset;  // �ϴ� ��迡 �����ϸ� ī�޶� ��ġ�� �÷��̾� �����ǿ��� ��¦ �Ʒ���
        }
        else
        {
            // �÷��̾ ���� ��� ���̿� ���� ��� ������ y ��ġ�� �ε巴�� ���ư�
            targetCamPos.y = Mathf.Lerp(transform.position.y, originalY, smoothing);
        }

        targetCamPos.z = transform.position.z;  // z���� �������� ����

        targetCamPos.x = Mathf.Clamp(targetCamPos.x, minCameraPos.x, maxCameraPos.x);
        targetCamPos.y = Mathf.Clamp(targetCamPos.y, minCameraPos.y, maxCameraPos.y);

        transform.position = Vector3.SmoothDamp(transform.position, targetCamPos, ref velocity, smoothing);
    }
}
