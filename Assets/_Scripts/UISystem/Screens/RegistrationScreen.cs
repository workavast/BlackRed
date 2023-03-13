using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegistrationScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;

    private string _playerName;
    private string _playerPassword;
    
    public void _Confirm()
    {
        if (_playerName.Length > 0 && _playerPassword.Length > 0)
        {
            bool playerRegistered= NetworkController.UserRegistration(_playerName, _playerPassword);

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

    public void _Quit()
    {
        Application.Quit();
    }
}
