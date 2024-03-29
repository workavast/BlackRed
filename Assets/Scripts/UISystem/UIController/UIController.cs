using UISystem.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private static GameObject UI_Activ;
    private static GameObject UI_PrevActiv;
    private static GameObject buffer;
    
    void Start()
    {
        if (UIScreenRepository.GetScreen<RegistrationScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<RegistrationScreen>().gameObject;
        else
        if (UIScreenRepository.GetScreen<EnteryScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<EnteryScreen>().gameObject;
        else
        if (UIScreenRepository.GetScreen<StartMenuScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<StartMenuScreen>().gameObject;
        else
        if (UIScreenRepository.GetScreen<MainMenuScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<MainMenuScreen>().gameObject;
        else 
        if (UIScreenRepository.GetScreen<LevelChoiceScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<LevelChoiceScreen>().gameObject;
        else 
        if (UIScreenRepository.GetScreen<GameplayScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<GameplayScreen>().gameObject;
        else 
        if (UIScreenRepository.GetScreen<FinishLevelScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<FinishLevelScreen>().gameObject;
        else 
        if (UIScreenRepository.GetScreen<FriendsScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<FriendsScreen>().gameObject;
        else 
        {
            Debug.LogError("Undefined screen or null");
            UI_Activ = null;
        }

        UI_PrevActiv = UI_Activ;
    }

    public static void SetWindow(Screen screen)
    {
        buffer = UI_Activ;
        UI_Activ.SetActive(false);

        switch (screen)
        {
            case Screen.Previous:
                UI_Activ = UI_PrevActiv; break;
            case Screen.RegistrationScreen:
                UI_Activ = UIScreenRepository.GetScreen<RegistrationScreen>().gameObject; break;
            case Screen.EntryScreen:
                UI_Activ = UIScreenRepository.GetScreen<EnteryScreen>().gameObject; break;
            case Screen.StartMenu:
                UI_Activ = UIScreenRepository.GetScreen<StartMenuScreen>().gameObject; break;
            case Screen.MainMenu:
                UI_Activ = UIScreenRepository.GetScreen<MainMenuScreen>().gameObject; break;
            case Screen.LevelChoiceScreen:
                UI_Activ = UIScreenRepository.GetScreen<LevelChoiceScreen>().gameObject; break;
            case Screen.GameplayScreen:
                UI_Activ = UIScreenRepository.GetScreen<GameplayScreen>().gameObject; break;
            case Screen.FinishLevelScreen:
                UI_Activ = UIScreenRepository.GetScreen<FinishLevelScreen>().gameObject; break;
            case Screen.GameplayMenuScreen:
                UI_Activ = UIScreenRepository.GetScreen<GameplayMenuScreen>().gameObject; break;   
            case Screen.ControlScreen:
                UI_Activ = UIScreenRepository.GetScreen<ControlScreen>().gameObject; break;
            case Screen.FriendsScreen:
                UI_Activ = UIScreenRepository.GetScreen<FriendsScreen>().gameObject; break;
            default:
                Debug.LogError("Error: invalid string parameter in SetWindow(Screen screen)"); break;
        }

        UI_PrevActiv = buffer;
        UI_Activ.SetActive(true);
    }

    public static void LoadScene(int sceneNum)
    {
        if (sceneNum == -1)
        {
            int currentSceneNum = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneNum, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneNum);
        }
    }

    public static void ShowError(string errorText)
    {
        ErrorScreen errorScreen = UIScreenRepository.GetScreen<ErrorScreen>();
        errorScreen.gameObject.SetActive(true);
        errorScreen.ShowError(errorText);
    }
    public static void HideError()
    {
        ErrorScreen errorScreen = UIScreenRepository.GetScreen<ErrorScreen>();
        errorScreen.gameObject.SetActive(false);
    }
    
    public static void Quit()
    {
        Application.Quit();
    }
}