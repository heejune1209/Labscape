using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float toggleInterval = 1.0f; // 레이저가 켜지고 꺼지는 간격
    private SpriteRenderer laserSprite; // 레이저의 스프라이트 렌더러
    private BoxCollider2D laserCollider; // 레이저의 콜라이더

    private bool isLaserActive = false; // 레이저의 활성 상태

    private void Awake()
    {
        laserCollider = GetComponent<BoxCollider2D>();
        laserSprite = GetComponent<SpriteRenderer>();
        // 레이저를 초기 상태로 설정
        laserSprite.enabled = false;
        laserCollider.enabled = false;
    }
    private void Start()
    {
        
        // 레이저의 켜짐과 꺼짐을 반복하는 코루틴 시작
        StartCoroutine(ToggleLaser());
    }

    private IEnumerator ToggleLaser()
    {
        while (true)
        {
            // 시작 시간을 저장
            float startTime = Time.time;
            // 레이저 상태 전환
            isLaserActive = !isLaserActive;
            laserSprite.enabled = isLaserActive;
            laserCollider.enabled = isLaserActive;

            // 다음 토글까지 대기
            while (Time.time < startTime + toggleInterval)
            {
                yield return null;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 레이저가 활성화된 상태에서 플레이어와 충돌했을 때
        if (isLaserActive && collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Die();
        }
    }
}
