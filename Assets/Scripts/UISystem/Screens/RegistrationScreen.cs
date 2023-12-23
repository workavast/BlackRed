using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegistrationScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }
    
    public void _Confirm()
    {
        if (playerName.text.Length > 0 && playerPassword.text.Length > 0)
        {
            NetworkController.Instance.UserRegistration(RegistrationCompleted, Error, playerName.text, playerPassword.text);
        }
    }
    
    private void RegistrationCompleted()
    {
        UIController.LoadScene(0);
    }
    
    private void Error(string errorText)
    {
        UIController.ShowError(errorText);
    }
}
