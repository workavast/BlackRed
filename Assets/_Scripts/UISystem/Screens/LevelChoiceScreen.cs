using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelChoiceScreen : UIScreenBase
{
    [SerializeField] private TMP_Text firstLevelTime;
    [SerializeField] private TMP_Text secondLevelTime;
    [SerializeField] private TMP_Text thirdLevelTime;

    void OnEnable()
    {

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
