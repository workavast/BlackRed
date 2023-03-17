using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private static GameObject UI_Activ;
    private static GameObject UI_PrevActiv;
    private static GameObject buffer;
    private static GameObject currentWorker;

    void Start()
    {
        if (UIScreenRepository.GetScreen<RegistrationScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<RegistrationScreen>().gameObject;
        else
        if (UIScreenRepository.GetScreen<EnteryScreen>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<EnteryScreen>().gameObject;
        else
        if (UIScreenRepository.GetScreen<StartMenu>().isActiveAndEnabled)
            UI_Activ = UIScreenRepository.GetScreen<StartMenu>().gameObject;
        else
        {
            Debug.LogError("Undefined screen or null");
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
                UI_Activ = UIScreenRepository.GetScreen<StartMenu>().gameObject; break;
            default:
                Debug.Log("Error: invalid string parameter in _SetWindow(string windowName)"); break;
        }

        UI_PrevActiv = buffer;
        UI_Activ.SetActive(true);
    }

    public static void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
    
    public static void Quit()
    {
        Application.Quit();
    }
}
