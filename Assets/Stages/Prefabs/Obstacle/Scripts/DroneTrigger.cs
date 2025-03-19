using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTrigger : MonoBehaviour
{
    public GameObject dronePrefab;  // 드론 프리팹
    public GameObject spawnPosition;   // 드론이 스폰될 위치
    public bool moveRight = true;   // 드론의 초기 이동 방향 설정
    public float detectionRadius = 5.0f;  // 드론의 플레이어 감지 반경

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
        droneController.isSpawned = true;  // 드론이 스폰되었음을 표시
    }
}
