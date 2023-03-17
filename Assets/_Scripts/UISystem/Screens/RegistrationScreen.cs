using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegistrationScreen : UIScreenBase
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
            bool playerRegistered= NetworkController.UserRegistration(playerName.text, playerPassword.text);

            if (playerRegistered)
            {
                Debug.Log("You are registered");
            }
            else
            {
                Debug.Log("Error with entered data");
            }
        }
    }
}
