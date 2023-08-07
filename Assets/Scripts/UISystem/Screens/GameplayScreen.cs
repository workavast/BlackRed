using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayScreen  : UIScreenBase
{
    [SerializeField] private TMP_Text fullLevelTime;
    private GhostSystem _ghostRecord;
    
    private void Start()
    {
        _ghostRecord = GhostSystem.This;
    }

    private void Update()
    {
        fullLevelTime.text = _ghostRecord.CurrentFullTime.ToString();
    }

    public void _SetWindow(int screen)
    {
        UIController.SetWindow((Screen)screen);
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }

    public void _Quit()
    {
        UIController.Quit();
    }
}