using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTrigger : MonoBehaviour
{
    public GameObject dronePrefab;  // ��� ������
    public GameObject spawnPosition;   // ����� ������ ��ġ
    public bool moveRight = true;   // ����� �ʱ� �̵� ���� ����
    public float detectionRadius = 5.0f;  // ����� �÷��̾� ���� �ݰ�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnDrone();
        }
    }

    void SpawnDrone()
    {
        GameObject drone = Instantiate(dronePrefab, spawnPosition.transform.position, Quaternion.identity);
        Drone droneController = drone.GetComponent<Drone>();
        droneController.moveRight = moveRight;
        droneController.detectionRadius = detectionRadius;
        droneController.isSpawned = true;  // ����� �����Ǿ����� ǥ��
    }
}
