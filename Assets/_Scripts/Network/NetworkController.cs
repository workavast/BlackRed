using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    private static string _playerName;
    private static string _playerPassword;
    private static int _playerID;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static bool EnterPlayer(string playerName, string playerPassword)
    {
        if (playerName == _playerName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }
    
    public static bool RegistrationNewPlayer(string playerName, string playerPassword)
    {
        if (playerName == _playerName && playerPassword == _playerPassword)
            return true;
        else
            return false;
    }
}
