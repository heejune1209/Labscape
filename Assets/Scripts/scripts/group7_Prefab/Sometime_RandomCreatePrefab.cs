using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 때때로 범위 내에 랜덤으로 프리팹을 만든다
public class Sometime_RandomCreatePrefab : MonoBehaviour
{

    public GameObject newPrefab; // 만드는 프리팹 ：Inspector에 지정한다
    public float intervalSec = 1; // 작성 간격（초）：Inspector로에 지정한다

    void Start()
    { // 처음에 시행한다
      // 지정 초 수마다 CreatePrefab를 반복 실행하는 예약
        InvokeRepeating("CreatePrefab", intervalSec, intervalSec);
    }

    void CreatePrefab()
    {
        // 이 오브젝트의 범위 내에 랜덤으로
        Vector3 area = GetComponent<SpriteRenderer>().bounds.size;

        Vector3 newPos = this.transform.position;
        newPos.x += Random.Range(-area.x / 2, area.x / 2);
        newPos.y += Random.Range(-area.y / 2, area.y / 2);
        newPos.z = -5; // 앞 쪽에 표시
                       // 프리팹을 만든다
        GameObject newGameObject = Instantiate(newPrefab) as GameObject;
        newGameObject.transform.position = newPos;
    }
}
