using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelChoiceScreen : UIScreenBase
{
    [SerializeField] private List<TMP_Text> levelsTimes;

    void OnEnable()
    {
        if(levelsTimes.Count != NetworkController.Levels.Count)
            Debug.LogError("Attention!: levelsTimes.Count != NetworkController.Levels.Count");
        
        for (int i = 0; i < levelsTimes.Count; i++)
        {
            if(NetworkController.Levels[i].time == 0)
                levelsTimes[i].text = "не пройдено";
            else
                levelsTimes[i].text = NetworkController.Levels[i].time.ToString();
        }
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