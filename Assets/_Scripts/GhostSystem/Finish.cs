using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.SetWindow(Screen.FinishLevelScreen);
            GhostRecord.GhostRecorder.StopRecord();
        }
    }
}