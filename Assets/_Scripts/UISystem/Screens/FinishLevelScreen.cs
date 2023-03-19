using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishLevelScreen : UIScreenBase
{
    [SerializeField] private TMP_Text fullLevelTime;
    [SerializeField] private TMP_Text timesDifference;

    private void OnEnable()
    {
        fullLevelTime.text = GhostRecord.CurrentFullTime.ToString();
        
        float difference = GhostRecord.PreviousFullTime - GhostRecord.CurrentFullTime;
        
        if(difference > 0)
            timesDifference.color = Color.green;
        else
            timesDifference.color = Color.red;
        
        timesDifference.text = difference.ToString();
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
