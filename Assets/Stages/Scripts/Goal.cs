using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject GoalTrigger;
    public End endscripts;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GoalTrigger.SetActive(true);
            Time.timeScale = 0f;
            endscripts.StageClear();
        }

    }
}
