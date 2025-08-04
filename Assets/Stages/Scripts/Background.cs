using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float movespeed = 0.5f; // ��� �̵� �ӵ�, ĳ���� �ӵ��� ����Ͽ� ���� ����
    public Transform player; // �÷��̾��� Transform�� ����
    private Vector3 previousPosition; // ���� �����ӿ����� �÷��̾� ��ġ

    void Start()
    {
        previousPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾��� ���� ��ġ�� ���� ��ġ�� ���̸� ���
        float moveDelta = player.position.x - previousPosition.x;

        // ��� ��ġ ������Ʈ, ĳ���� �̵� ������ �ݴ� �������� ����� ������
        transform.position -= new Vector3(moveDelta * movespeed, 0, 0);

        // ���� �÷��̾� ��ġ�� ����
        previousPosition = player.position;
        /*
        if(transform.position.x < -18)
        {
            transform.position += new Vector3(36f, 0, 0);
        }
        */
    }
}
