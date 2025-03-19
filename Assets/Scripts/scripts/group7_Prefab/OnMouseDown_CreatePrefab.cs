using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 거기에 프리팹을 만든다
public class OnMouseDown_CreatePrefab : MonoBehaviour
{

    public GameObject newPrefab; // 만드는 프리팹：Inspector에 지정

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 터치한 위치를 카메라 안에서의 위치로 변환해서
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
            pos.z = -5; // 앞쪽에 표시
                        // 새로운 프리팹을 만들어 그 위치로 이동한다 
            GameObject newGameObject = Instantiate(newPrefab) as GameObject;
            newGameObject.transform.position = pos;
        }
    }
}
