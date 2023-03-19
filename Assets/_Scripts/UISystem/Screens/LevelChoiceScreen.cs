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
        float firstLevel = NetworkController.Levels[0].time;
        float secondLevel = NetworkController.Levels[1].time;
        float thirdLevel = NetworkController.Levels[2].time;

        if (firstLevel == 0)
            firstLevelTime.text = "не пройдено";
        else
            firstLevelTime.text = firstLevel.ToString();
        
        if (secondLevel == 0)
            secondLevelTime.text = "не пройдено";
        else
            secondLevelTime.text = firstLevel.ToString();

        if (thirdLevel == 0)
            thirdLevelTime.text = "не пройдено";
        else
            thirdLevelTime.text = firstLevel.ToString();
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
