using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    public List<GameObject> Doors;
    public float Closetime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        // 게임 시작 시 모든 문의 BoxCollider2D를 비활성화
        foreach (GameObject door in Doors)
        {
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false; // BoxCollider2D 비활성화
                door.SetActive(false); // 문 자체도 비활성화
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 targettrigger에 닿았을 때 검사
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("TargetTrigger"))
        {
            StartCoroutine(ActivateDoorsAfterDelay1(Closetime)); // 1초 후에 문을 활성화
        }
    }
    IEnumerator ActivateDoorsAfterDelay1(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간(초) 동안 대기
        foreach (GameObject door in Doors)
        {
            door.SetActive(true); // 각 문 활성화
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true; // BoxCollider2D 활성화

            }
        }        
    }

}
