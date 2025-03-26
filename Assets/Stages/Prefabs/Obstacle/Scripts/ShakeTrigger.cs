using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    public CameraShake cameraShake;             // ī�޶� ���� ��ũ��Ʈ
    public float ShakeTime = 0.2f;
    public float magnitude = 0.3f;
    public PlayerController _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(cameraShake.Shake(ShakeTime, magnitude)); // ���� �ð��� ���� ����
            _player.GetComponent<PlayerController>().Die();           
        }
        if (collision.gameObject.CompareTag("Invincibility") && gameObject.CompareTag("Obstacle"))
        {
            gameObject.tag = "Trigger";
        }
    }
}
