using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    public void _LoadLevel(int levelNum)
    {
        NetworkController.TakePoints(TakePointsComplete, levelNum);
    }

    private void TakePointsComplete(int levelNum)
    {
        int n;
        switch (levelNum)
        {
            case 1:
            {
                n = 2;
                break;
            }
            case 2:
            {
                n = 3;
                break;
            }
            case 3:
            {
                n = 4;
                break;
            }
            default:
            {
                Debug.LogError("Unexpected level number");
                return;
            }
        }
        
        _LoadScene(n);
    }
    
    public void _Quit()
    {
        UIController.Quit();
    }
}