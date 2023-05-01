using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SQL_Classes;
using UnityEngine.Serialization;

public class NetworkController : MonoBehaviour
{
    private enum Commands
    {
        UserEnter,
        UserRegistration,
        UpdateLevelTime,
        SaveWay,
        TakeWays,
        TakeLeaderboard
    }
    
    public static NetworkController Instance { get; private set;} 
    
    private User _user;
    private Way _playerWay = new Way();
    private Ways _otherWays = new Ways();
    
    public string UserName => _user.name;
    public List<Level> Levels => _user.levels;
    public List<Point> PlayerPoints => _playerWay.points;
    public Ways OtherWays => _otherWays;

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
        _user = new User();
        _playerWay = new Way();
        _otherWays = new Ways();
    }


    private void ErrorCode(long errorNum)
    {
        switch (errorNum)
        {
            case (0):
            {
                Debug.LogError("No connection");
                break;
            }
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
            ErrorCode(www.responseCode);
        }
        else
        {
            var json = www.downloadHandler.text;
            _user = JsonUtility.FromJson<User>(json);
            
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
            ErrorCode(www.responseCode);
        }
        else
        {
            var json = www.downloadHandler.text;
            _user = JsonUtility.FromJson<User>(json);

            funcComplete.Invoke();
        }
        
        www.Dispose();
    }

    
    
    public void UpdateLevelTime(System.Action funcComplete, int levelNum, float time)
    {
        StartCoroutine(UpdateLevelTimeCoroutine(funcComplete, _user.id, levelNum, time));
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
            ErrorCode(www.responseCode);
        }
        else
        {
            var json = www.downloadHandler.text;
            
            _user.levels[levelNum-1].time = time;
            
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    
    public void SaveWay(System.Action funcComplete, int levelNum, Way way)
    {
        StartCoroutine(SaveWayCoroutine(funcComplete, _user.id, levelNum, way));
    }
    
    private IEnumerator SaveWayCoroutine(System.Action funcComplete, int user_id, int levelNum, Way way)
    {
        string wayJSON = JsonUtility.ToJson(way);
        
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.SaveWay.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));
        wwwForm.Add(new MultipartFormDataSection("way", wayJSON));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            ErrorCode(www.responseCode);
        }
        else
        {
            _playerWay = way;
            
            Debug.LogError( www.downloadHandler.text);
            
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    
    public void TakeWays(System.Action<int> funcComplete, int levelNum, float time)
    {
        StartCoroutine(TakeWaysCoroutine(funcComplete, _user.id, levelNum, time));
    }

    private IEnumerator TakeWaysCoroutine(System.Action<int> funcComplete, int user_id, int levelNum, float time)
    {
        if (Levels[levelNum - 1].time == 0)
        {
            _playerWay = new Way();
            _otherWays = new Ways();
            
            funcComplete.Invoke(levelNum);
            
            yield break;
        }

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
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakeWays.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelName", levelName));
        wwwForm.Add(new MultipartFormDataSection("time", timeString));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            ErrorCode(www.responseCode);
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
            
            Ways ways = new Ways();
            ways = JsonUtility.FromJson<Ways>(json);

            _playerWay = ways.ways[0];
            ways.ways.RemoveAt(0);
            _otherWays = ways;

            funcComplete.Invoke(levelNum);
        }
        
        www.Dispose();
    }


    
    public void TakeLeaderboard(System.Action<Leaderboard> funcComplete, int levelNum)
    {
        StartCoroutine(TakeLeaderboardCoroutine(funcComplete, _user.id, levelNum));
    }
    
    private IEnumerator TakeLeaderboardCoroutine(System.Action<Leaderboard> funcComplete, int user_id, int levelNum)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakeLeaderboard.ToString()));
        wwwForm.Add(new MultipartFormDataSection("user_id", user_id.ToString()));
        wwwForm.Add(new MultipartFormDataSection("levelNum", levelNum.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        yield return www.SendWebRequest();
        
        if (www.error != null)
        {
            ErrorCode(www.responseCode);
        }
        else
        {
            string json = www.downloadHandler.text;
            
            Leaderboard ld = JsonUtility.FromJson<Leaderboard>(json);
            
            funcComplete.Invoke(ld);
        }
        
        www.Dispose();
    }
}