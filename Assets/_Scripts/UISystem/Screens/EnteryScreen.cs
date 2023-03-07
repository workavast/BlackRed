using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnteryScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;

    private string _playerName;
    private string _playerPassword;
    
    public void _Confirm()
    {
        if (_playerName.Length > 0 && _playerPassword.Length > 0)
        {
            bool playerEntered = NetworkController.EnterPlayer(_playerName, _playerPassword);

            if (playerEntered)
            {
                Debug.Log("You are entered");
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
