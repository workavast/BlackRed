using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class User
{
    public int id;
    public string name;
}

enum Commands
{
    UserEnter,
    UserRegistration
}
public class NetworkController : MonoBehaviour
{
    private static User user;
    public static string UserName => user.name;
    private static string _playerPassword;
    private static int _playerID;

    private static NetworkController _networkController;
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _networkController = this;
    }

    void Update()
    {
        
    }

    public static bool UserEnter(string playerName, string playerPassword)
    {
        _networkController.StartUserEnterCoroutine(playerName, playerPassword);
        
        if (playerName == UserName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }

    private void StartUserEnterCoroutine(string playerName, string playerPassword)
    {
        StartCoroutine(UserEnterCoroutine(playerName, playerPassword));
    }
    
    private IEnumerator UserEnterCoroutine(string playerName, string playerPassword)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.UserEnter.ToString()));
        wwwForm.Add(new MultipartFormDataSection("name", playerName));
        wwwForm.Add(new MultipartFormDataSection("password", playerPassword));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            switch (www.responseCode)
            {
                case (401):
                {
                    Debug.Log("Not correct password");
                    break;
                }
                case (404):
                {
                    Debug.Log("This name dont exist");
                    break;
                }
                default:
                {
                    Debug.Log("Unexpected error");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Completed");
            var json = www.downloadHandler.text;

            user = JsonUtility.FromJson<User>(json);
            
            UIController.SetWindow(Screen.MainMenu);
            
            Debug.Log(user.id);
            Debug.Log(user.name);
        }
        
        www.Dispose();
    }
    
    public static bool UserRegistration(string playerName, string playerPassword)
    {
        _networkController.StartUserRegistrationCoroutine(playerName, playerPassword);
        if (playerName == UserName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }
    
    private void StartUserRegistrationCoroutine(string playerName, string playerPassword)
    {
        StartCoroutine(UserRegistrationCoroutine(playerName, playerPassword));
    }
    
    private IEnumerator UserRegistrationCoroutine(string playerName, string playerPassword)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.UserRegistration.ToString()));
        wwwForm.Add(new MultipartFormDataSection("name", playerName));
        wwwForm.Add(new MultipartFormDataSection("password", playerPassword));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            switch (www.responseCode)
            {
                case (409):
                {
                    Debug.Log("This name is occupied");
                    break;
                }
                case (520):
                {
                    Debug.Log("Some error, try again");
                    break;
                }
                default:
                {
                    Debug.Log("Unexpected error");
                    break;
                }
            }        
        }
        else
        {
            Debug.Log("Completed");
            var json = www.downloadHandler.text;

            user = JsonUtility.FromJson<User>(json);
            Debug.Log(user.id);
            Debug.Log(user.name);
            
            UIController.SetWindow(Screen.MainMenu);
        }
        
        www.Dispose();
    }
}
