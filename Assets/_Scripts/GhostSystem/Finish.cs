using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] [Range(1,3)] private int levelNum;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.SetWindow(Screen.FinishLevelScreen);
            GhostRecord.StopRecord();
            Debug.LogError(GhostRecord.FullTime);
            if (NetworkController.Levels[levelNum - 1].time != 0)
            {
                if(GhostRecord.FullTime < NetworkController.Levels[levelNum-1].time)
                    NetworkController.UpdateLevelTime(levelNum,GhostRecord.FullTime);
            }
            else
            {
                NetworkController.UpdateLevelTime(levelNum,GhostRecord.FullTime);
            }

        }
    }
}
