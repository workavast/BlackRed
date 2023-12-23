using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuScreen : UIScreenBase
{
    [SerializeField] private TMP_Text playerName;

    void OnEnable()
    {
        playerName.text = NetworkController.Instance.UserName;
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
