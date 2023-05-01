using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using SQL_Classes;
using UnityEngine.Serialization;

public class LevelChoiceScreen : UIScreenBase
{
    [Serializable]
    private struct PlayerBord
    {
        public TextMeshProUGUI place;
        public TextMeshProUGUI name;
        public TextMeshProUGUI time;
    }
    
    [SerializeField] private GameObject levelInformation;
    [SerializeField] private GameObject loadLevelInformation;
    
    [Space]
    [SerializeField] private Sprite redMedal;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite copperMedal;

    [Space]
    [SerializeField] private Image currentMedal;

    [Space]
    [SerializeField] private TextMeshProUGUI currentTime;
    [SerializeField] private TextMeshProUGUI goldTime;
    [SerializeField] private TextMeshProUGUI silverTime;
    [SerializeField] private TextMeshProUGUI copperTime;
    
    [Space]
    [SerializeField] private List<PlayerBord> playersBoards;

    private int _loadLevelNum;
    
    void OnEnable()
    {
        levelInformation.SetActive(false);

        foreach (var board in playersBoards)
        {
            board.place.text = "";
            board.name.text = "";
            board.time.text = "";
        }
    }

    public void _LoadLevelInformation(int levelNum)
    {
        if(_loadLevelNum == levelNum)
            return;

        _loadLevelNum = levelNum;
        
        levelInformation.SetActive(true);
        loadLevelInformation.SetActive(true);
        
        if(NetworkController.Instance.Levels[levelNum - 1].time == 0)
            currentTime.text = "не пройдено";
        else
            currentTime.text = NetworkController.Instance.Levels[levelNum - 1].time.ToString();
        
        foreach (var board in playersBoards)
        {
            board.place.text = "";
            board.name.text = "";
            board.time.text = "";
        }

        if(NetworkController.Instance.Levels[levelNum - 1].time != 0)
            NetworkController.Instance.TakeLeaderboard(LoadingLevelInformation, Error, levelNum);
    }

    private void LoadingLevelInformation(Leaderboard leaderboard)
    {
        loadLevelInformation.SetActive(false);
        for (int n = 0; n < leaderboard.boards.Count; n++)
        {
            playersBoards[n].place.text = leaderboard.boards[n].place.ToString();
            playersBoards[n].name.text =  leaderboard.boards[n].name;
            playersBoards[n].time.text =  leaderboard.boards[n].time.ToString();
        }
    }
    
    public void _SetWindow(int screen)
    {
        UIController.SetWindow((Screen)screen);
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }

    public void _LoadLevel()
    {
        NetworkController.Instance.TakeWays(TakeWaysComplete, Error, _loadLevelNum, NetworkController.Instance.Levels[_loadLevelNum - 1].time);

    }
    
    private void TakeWaysComplete(int levelNum)
    {
        int n;
        switch (levelNum)
        {
            case 1:
            {
                n = 2;
                break;
            }
            case 2:
            {
                n = 3;
                break;
            }
            case 3:
            {
                n = 4;
                break;
            }
            default:
            {
                Debug.LogError("Unexpected level number");
                return;
            }
        }
        
        _LoadScene(n);
    }

    private void Error(string errorText)
    {
        UIController.ShowError(errorText);
    }
}