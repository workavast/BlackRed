using UnityEngine;
using TMPro;
using WEB_API;

public class EnteryScreen : UIScreenBase
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
            GlobalData.Instance.NetworkController.UserLogin(EnterCompleted, Error, playerName.text, playerPassword.text);
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