using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RegistrationScreen : UIScreenBase
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;

    public void _Confirm()
    {
        
    }

    public void _Quit()
    {
        Application.Quit();
    }
}
