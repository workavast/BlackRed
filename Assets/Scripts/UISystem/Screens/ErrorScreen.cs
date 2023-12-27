using UnityEngine;
using TMPro;

public class ErrorScreen : UIScreenBase
{
    [SerializeField] private TextMeshProUGUI textMessage;
    
    public void ShowError(string errorText)
    {
        textMessage.text = errorText;
    }
    
    public void _HideError()
    {
        UIController.HideError();
    }
}
