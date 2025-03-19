using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f; // 플레이어를 위로 날려보낼 힘
    public GameObject spring;
    public bool springInitiallyActive = false; // 에디터에서 설정할 수 있는 초기 상태 변수
    //public float ReenableTime = 4f;
    //public Button targetButton; // 비활성화할 버튼 오브젝트

    private void Start()
    {
        spring.SetActive(springInitiallyActive);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            Collider2D playerCollider = collision.GetComponent<Collider2D>();;
            if (playerRb != null && playerCollider != null)
            {
                // 용수철의 힘으로 플레이어를 위로 날려보냄
                spring.SetActive(true);
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Block"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Elevator"), true);
                // 플레이어의 모든 충돌을 일시적으로 무시
                //playerCollider.enabled = false;
                //targetButton.interactable = false; // 버튼 비활성화
                // 일정 시간 후에 플레이어의 충돌을 다시 활성화
                //StartCoroutine(ReenableCollision(playerCollider, ReenableTime)); // 1.5초 후에 충돌 다시 활성화
            }
        }
    }
    /*
    IEnumerator ReenableCollision(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        //collider.enabled = true;
        //targetButton.interactable = true; // 버튼 다시 활성화
    }
    */
}
