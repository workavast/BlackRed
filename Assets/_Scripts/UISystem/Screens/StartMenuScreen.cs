using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenuScreen : UIScreenBase
{
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
