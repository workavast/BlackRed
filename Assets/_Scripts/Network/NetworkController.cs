using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization.Json;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;
using SQL_Points;

[System.Serializable]
public class User
{
    public int id;
    public string name;
    public List<Level> levels = new List<Level>();
}

[System.Serializable]
public class Level
{
    public int num;
    public float time;
}

enum Commands
{
    UserEnter,
    UserRegistration,
    UpdateLevelTime,
    SavePoints
}

public class NetworkController : MonoBehaviour
{
    private static User user;
    public static string UserName => user.name;
    public static List<Level> Levels => user.levels;
    private static NetworkController _networkController;
    
    void Start()
    {
        if (_networkController != null)
        {
            Destroy(this);
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
        _networkController = this;
    }
    
    public static bool UserEnter(string playerName, string playerPassword)
    {
        _networkController.StartUserEnterCoroutine(playerName, playerPassword);
        
        if (playerName == "UserName")
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
                    Debug.LogError("Not correct password");
                    break;
                }
                case (404):
                {
                    Debug.LogError("This name dont exist");
                    break;
                }
                default:
                {
                    Debug.LogError("Unexpected error");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Completed");
            
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);
            
            UIController.LoadScene(0);
        }
        
        www.Dispose();
    }
    
    
    public static bool UserRegistration(string playerName, string playerPassword)
    {
        _networkController.StartUserRegistrationCoroutine(playerName, playerPassword);
        if (playerName == "UserName")
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
                    Debug.LogError("This name is occupied");
                    break;
                }
                case (520):
                {
                    Debug.LogError("Some error, try again");
                    break;
                }
                default:
                {
                    Debug.LogError("Unexpected error");
                    break;
                }
            }        
        }
        else
        {
            Debug.Log("Completed");
            
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);

            UIController.LoadScene(0);
        }
        
        www.Dispose();
    }

    
    public static void UpdateLevelTime(int levelNum, float time)
    {
        _networkController.StartUpdateLevelTimeCoroutine(user.id, levelNum, time);
    }
    
    private void StartUpdateLevelTimeCoroutine(int user_id, int levelNum, float time)
    {
        StartCoroutine(UpdateLevelTimeCoroutine(user_id, levelNum, time));
    }
    
    private IEnumerator UpdateLevelTimeCoroutine(int user_id, int levelNum, float time)
    {
        string levelName;
        switch (levelNum)
        {
            case 1: levelName = "level_1"; break;
            case 2: levelName = "level_2"; break;
            case 3: levelName = "level_3"; break;
            default: Debug.LogError("Not exist num"); yield break;
        }
        
        string timeString = time.ToString();
        for (int n = 0; n < timeString.Length; n++)
        {
            if (timeString[n] == ',')
            {
                timeString = timeString.Remove(n,1);
                timeString = timeString.Insert(n,".");
            }
        }
        
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.UpdateLevelTime.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelName", levelName));
        wwwForm.Add(new MultipartFormDataSection("time", timeString));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            switch (www.responseCode)
            {
                case (520):
                {
                    Debug.LogError("Some error, try again");
                    break;
                }
                default:
                {
                    Debug.LogError("Unexpected error");
                    break;
                }
            }        
        }
        else
        {
            Debug.Log("Completed");
            
            var json = www.downloadHandler.text;
            
            user.levels[levelNum-1].time = time;
        }
        
        www.Dispose();
    }
    
    
    public static void SavePoints(int levelNum, Points points)
    {
        _networkController.StartSavePointsCoroutine(user.id, levelNum, points);
    }
    
    private void StartSavePointsCoroutine(int user_id, int levelNum, Points points)
    {
        StartCoroutine(SavePointsCoroutine(user_id, levelNum, points));
    }
    
    private IEnumerator SavePointsCoroutine(int user_id, int levelNum, Points points)
    {
        string pointsJSON = JsonUtility.ToJson(points);
        
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.SavePoints.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));
        wwwForm.Add(new MultipartFormDataSection("points", pointsJSON));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            switch (www.responseCode)
            {
                case (520):
                {
                    Debug.LogError("Some error, try again");
                    break;
                }
                default:
                {
                    Debug.LogError("Unexpected error");
                    break;
                }
            }        
        }
        else
        {
            Debug.Log("Completed");
            Debug.LogError( www.downloadHandler.text);
        }
        
        www.Dispose();
    }
}