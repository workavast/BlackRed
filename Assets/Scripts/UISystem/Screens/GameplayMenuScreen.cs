using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMenuScreen : UIScreenBase
{
    private float _timeScale;
    void OnEnable()
    {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = _timeScale;
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