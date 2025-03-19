using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroy : MonoBehaviour
{
    public float deleteTime = 5.0f;
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
