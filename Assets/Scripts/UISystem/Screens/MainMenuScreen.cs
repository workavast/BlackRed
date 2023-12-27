using UnityEngine;
using TMPro;
using WEB_API;

public class MainMenuScreen : UIScreenBase
{
    [SerializeField] private TMP_Text playerName;

    void OnEnable()
    {
        playerName.text = GlobalData.Instance.PlayerDataStorage.Name;
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }

    public void _Quit()
    {
        UIController.Quit();
    }
}
