using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnteryScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;

    public void _SetWindow(int screen)
    {
        UIController.SetWindow((Screen)screen);
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }
    
    public void _Confirm()
    {
        if (playerName.text.Length > 0 && playerPassword.text.Length > 0)
        {
             NetworkController.Instance.UserEnter(EnterCompleted, Error, playerName.text, playerPassword.text);
        }
    }

    private void EnterCompleted()
    {
        UIController.LoadScene(0);
    }
    
    private void Error(string errorText)
    {
        UIController.ShowError(errorText);
    }
}