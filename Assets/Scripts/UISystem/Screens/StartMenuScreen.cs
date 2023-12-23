using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenuScreen : UIScreenBase
{
    private void Start()
    {
        NetworkController.Instance.Clear();
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
