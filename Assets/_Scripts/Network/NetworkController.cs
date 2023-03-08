using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour
{
    private static string _playerName;
    private static string _playerPassword;
    private static int _playerID;

    private static NetworkController _networkController;
    
    void Start()
    {
        _networkController = this;
    }

    void Update()
    {
        
    }

    public static bool EnterPlayer(string playerName, string playerPassword)
    {
        _networkController.StartCheck(playerName, playerPassword);
        
        if (playerName == _playerName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }

    private void StartCheck(string playerName, string playerPassword)
    {
        StartCoroutine(_networkController.Check(playerName, playerPassword));
    }
    
    private IEnumerator Check(string playerName, string playerPassword)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("name", playerName));
        wwwForm.Add(new MultipartFormDataSection("password", playerPassword));

        UnityWebRequest www = UnityWebRequest.Post("blackredgame.loc", wwwForm);
        
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            //byte[] result = www.downloadHandler.data;
        }
        
        www.Dispose();
    }
    
    public static bool RegistrationNewPlayer(string playerName, string playerPassword)
    {
        if (playerName == _playerName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }
}
