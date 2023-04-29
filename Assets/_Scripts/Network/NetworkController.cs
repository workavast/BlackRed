using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SQL_Classes;

public class NetworkController : MonoBehaviour
{
    private enum Commands
    {
        UserEnter,
        UserRegistration,
        UpdateLevelTime,
        SaveWay,
        TakePlayerWay,
        TakeNearWays
    }
    
    public static NetworkController Instance { get; private set;} 
    
    private User user;
    public string UserName => user.name;
    public List<Level> Levels => user.levels;

    private Points _playerPoints = new Points();
    public List<Point> PlayerPoints => _playerPoints.points;

    public Ways OtherPlayersPoints = new Ways();

    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }


    public void Clear()
    {
        user = new User();
        _playerPoints = new Points();
        OtherPlayersPoints = new Ways();
    }
    
    
    public void UserEnter(System.Action funcComplete, string playerName, string playerPassword)
    {
        StartCoroutine(UserEnterCoroutine(funcComplete, playerName, playerPassword));
    }

    private IEnumerator UserEnterCoroutine(System.Action funcComplete, string playerName, string playerPassword)
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
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);
            
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    public void UserRegistration(System.Action funcComplete,string playerName, string playerPassword)
    {
        StartCoroutine(UserRegistrationCoroutine(funcComplete, playerName, playerPassword));
    }
    
    private IEnumerator UserRegistrationCoroutine(System.Action funcComplete,string playerName, string playerPassword)
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
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);

            funcComplete.Invoke();
        }
        
        www.Dispose();
    }

    
    public void UpdateLevelTime(System.Action funcComplete, int levelNum, float time)
    {
        StartCoroutine(UpdateLevelTimeCoroutine(funcComplete, user.id, levelNum, time));
    }
    
    private IEnumerator UpdateLevelTimeCoroutine(System.Action funcComplete, int user_id, int levelNum, float time)
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
            var json = www.downloadHandler.text;
            
            user.levels[levelNum-1].time = time;
            
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    public void SaveWay(System.Action funcComplete, int levelNum, Points points)
    {
        StartCoroutine(SaveWayCoroutine(funcComplete, user.id, levelNum, points));
    }
    
    private IEnumerator SaveWayCoroutine(System.Action funcComplete, int user_id, int levelNum, Points points)
    {
        string pointsJSON = JsonUtility.ToJson(points);
        
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.SaveWay.ToString()));
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
            _playerPoints = points;
            Debug.LogError( www.downloadHandler.text);
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    public void TakePlayerWay(System.Action<int> funcComplete, int levelNum)
    {
        StartCoroutine(TakePlayerWayCoroutine(funcComplete, user.id, levelNum));
    }

    private IEnumerator TakePlayerWayCoroutine(System.Action<int> funcComplete, int user_id, int levelNum)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakePlayerWay.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));

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
            string json = www.downloadHandler.text;
            _playerPoints = JsonUtility.FromJson<Points>(json);
            
            funcComplete.Invoke(levelNum);
        }
        
        www.Dispose();
    }
    
    
    
    public void TakeNearWays(System.Action<int> funcComplete, int levelNum, float time)
    {
        StartCoroutine(TakeNearWaysCoroutine(funcComplete, levelNum, time));
    }

    private IEnumerator TakeNearWaysCoroutine(System.Action<int> funcComplete, int levelNum, float time)
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
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakeNearWays.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));
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
                    string json = www.downloadHandler.text;
                    Debug.Log(json);
                    break;
                }
            }
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
            
            OtherPlayersPoints = JsonUtility.FromJson<Ways>(json);
            
            funcComplete.Invoke(levelNum);
        }
        
        www.Dispose();
    }
}