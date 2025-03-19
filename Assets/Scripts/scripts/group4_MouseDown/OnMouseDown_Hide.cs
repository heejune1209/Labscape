using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터치하면 지운다
public class OnMouseDown_Hide : MonoBehaviour
{

    void OnMouseDown()// 터치하면
    { 
        this.gameObject.SetActive(false); // 지운다
    }
}
