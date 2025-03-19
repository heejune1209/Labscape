using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    public CameraShake cameraShake;             // 카메라 진동 스크립트
    public float ShakeTime = 0.2f;
    public float magnitude = 0.3f;
    public PlayerController _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(cameraShake.Shake(ShakeTime, magnitude)); // 진동 시간과 강도 설정
            _player.GetComponent<PlayerController>().Die();           
        }
        if (collision.gameObject.CompareTag("Invincibility") && gameObject.CompareTag("Obstacle"))
        {
            gameObject.tag = "Trigger";
        }
    }
}
