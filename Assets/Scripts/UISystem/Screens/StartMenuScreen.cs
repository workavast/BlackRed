using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEB_API;

public class StartMenuScreen : UIScreenBase
{
    private void Start()
    {
        GlobalData.Instance.NetworkController.Clear();
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
