using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SQL_Classes;

enum Commands
{
    UserEnter,
    UserRegistration,
    UpdateLevelTime,
    SavePoints,
    TakePoints,
    TakeNearWay
}

public class NetworkController : MonoBehaviour
{
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
    
    
    public void UserEnter(string playerName, string playerPassword)
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
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);
            
            UIController.LoadScene(0);
        }
        
        www.Dispose();
    }
    
    
    public void UserRegistration(string playerName, string playerPassword)
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
            var json = www.downloadHandler.text;
            user = JsonUtility.FromJson<User>(json);

            UIController.LoadScene(0);
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
    
    
    public void SavePoints(System.Action funcComplete, int levelNum, Points points)
    {
        StartCoroutine(SavePointsCoroutine(funcComplete, user.id, levelNum, points));
    }
    
    private IEnumerator SavePointsCoroutine(System.Action funcComplete, int user_id, int levelNum, Points points)
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
            _playerPoints = points;
            Debug.LogError( www.downloadHandler.text);
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
    
    
    public void TakePoints(System.Action<int> funcComplete, int levelNum)
    {
        StartCoroutine(TakePointsCoroutine(funcComplete, user.id, levelNum));
    }

    private IEnumerator TakePointsCoroutine(System.Action<int> funcComplete, int user_id, int levelNum)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakePoints.ToString()));
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
    
    
    
    public void TakeNearWay(System.Action funcComplete, int levelNum, float time)
    {
        StartCoroutine(TakeNearWayCoroutine(funcComplete, levelNum, time));
    }

    private IEnumerator TakeNearWayCoroutine(System.Action funcComplete, int levelNum, float time)
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
        wwwForm.Add(new MultipartFormDataSection("Command", Commands.TakeNearWay.ToString()));
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
            
            funcComplete.Invoke();
        }
        
        www.Dispose();
    }
}