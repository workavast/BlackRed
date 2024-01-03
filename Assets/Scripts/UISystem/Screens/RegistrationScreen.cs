using UnityEngine;
using TMPro;
using WEB_API;

public class RegistrationScreen : UIScreenBase
{
    [SerializeField] private GameObject loading;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_InputField playerPassword;

    private void Awake()
    {
        SwitchLoadingVisible(false);
    }

    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }
    
    public void _Confirm()
    {
        if (playerName.text.Length <= 0 || playerPassword.text.Length <= 0) return;
        SwitchLoadingVisible(true);

        GlobalData.Instance.NetworkController.UserRegistration(RegistrationCompleted, OnError, playerName.text,
            playerPassword.text);
    }

    private void RegistrationCompleted() => GlobalData.Instance.NetworkController.TakePlayerLevelsData(LoadPlayerDataCompleted, OnError);
    
    private void LoadPlayerDataCompleted()
    {
        SwitchLoadingVisible(false);
        UIController.LoadScene(0);
    }

    private void OnError(string errorText)
    {
        SwitchLoadingVisible(false);
        UIController.ShowError(errorText);
    }

    private void SwitchLoadingVisible(bool show) => loading.SetActive(show);
}
