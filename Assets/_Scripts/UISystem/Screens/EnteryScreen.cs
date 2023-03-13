using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnteryScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;
    
    public void _Confirm()
    {
        if (playerName.text.Length > 0 && playerPassword.text.Length > 0)
        {
            bool playerEntered = NetworkController.UserEnter(playerName.text, playerPassword.text);

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
