using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float movespeed = 0.5f; // 배경 이동 속도, 캐릭터 속도에 비례하여 조정 가능
    public Transform player; // 플레이어의 Transform을 참조
    private Vector3 previousPosition; // 이전 프레임에서의 플레이어 위치

    void Start()
    {
        previousPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어의 현재 위치와 이전 위치의 차이를 계산
        float moveDelta = player.position.x - previousPosition.x;

        // 배경 위치 업데이트, 캐릭터 이동 방향의 반대 방향으로 배경을 움직임
        transform.position -= new Vector3(moveDelta * movespeed, 0, 0);

        // 현재 플레이어 위치를 저장
        previousPosition = player.position;
        /*
        if(transform.position.x < -18)
        {
            transform.position += new Vector3(36f, 0, 0);
        }
        */
    }
}
