using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using SQL_Classes;
using WEB_API;

public class LevelChoiceScreen : UIScreenBase
{
    [Serializable]
    private struct PlayerBord
    {
        public TextMeshProUGUI place;
        public TextMeshProUGUI name;
        public TextMeshProUGUI time;
    }
    
    [SerializeField] private GameObject playerFrame;
    [SerializeField] private GameObject levelInformation;
    [SerializeField] private GameObject loadLeaderboard;
    [SerializeField] private GameObject closeLeaderboard;
    
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

    private GlobalData _globalData => GlobalData.Instance;
    
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

        var res = _globalData.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == levelNum);
        if (res is null)
        {
            currentTime.text = "не пройдено";
            closeLeaderboard.SetActive(true);
        }
        else
        {
            currentTime.text = res.Time.ToString();
            closeLeaderboard.SetActive(false);
            loadLeaderboard.SetActive(true);
        }
        
        playerFrame.SetActive(false);
        foreach (var board in playersBoards)
        {
            board.place.text = "";
            board.name.text = "";
            board.time.text = "";
        }

        if (_globalData.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == levelNum) != null)
            _globalData.NetworkController.TakeLeaderboardPage(LoadingLevelInformation, Error, levelNum);
    }

    private void LoadingLevelInformation(LeaderboardPage leaderboardPage)
    {
        loadLeaderboard.SetActive(false);
        for (int n = 0; n < leaderboardPage.Rows.Count; n++)
        {
            playersBoards[n].place.text = leaderboardPage.Rows[n].Place.ToString();
            playersBoards[n].name.text =  leaderboardPage.Rows[n].UserName;
            playersBoards[n].time.text =  leaderboardPage.Rows[n].Time.ToString();
            
            if (playersBoards[n].name.text == _globalData.PlayerDataStorage.Name)
            {
                playerFrame.SetActive(true);
                Vector3 startPos = playerFrame.transform.position;
                startPos.y = playersBoards[n].name.transform.position.y;
                playerFrame.transform.position = startPos;
            }
        }
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }

    
    public void _LoadLevel()
    {
        _globalData.NetworkController.TakeNearWays(TakeWaysComplete, Error, _loadLevelNum);
    }
    
    private void TakeWaysComplete()
    {
        int n;
        switch (_loadLevelNum)
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