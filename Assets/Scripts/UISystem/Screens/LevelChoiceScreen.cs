using System;
using System.Linq;
using DataStorages;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using UISystem.Elements;
using WebApiConverters;

public class LevelChoiceScreen : UIScreenBase
{
    [SerializeField] private GameObject levelInformation;
    
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
    [SerializeField] private LeaderboardPage_UI globalLeaderboard;
    [SerializeField] private LeaderboardPage_UI friendsLeaderboard;

    private int _loadLevelNum;

    private GlobalData _globalData => GlobalData.Instance;
    
    void OnEnable()
    {
        levelInformation.SetActive(false);
        globalLeaderboard.Init();
        friendsLeaderboard.Init();
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
            globalLeaderboard.SwitchCloseScreenVisible(true);
            friendsLeaderboard.SwitchCloseScreenVisible(true);
            currentTime.text = "не пройдено";
            return;
        }

        globalLeaderboard.SwitchCloseScreenVisible(false);
        friendsLeaderboard.SwitchCloseScreenVisible(false);
        
        currentTime.text = res.Time.ToString();
        
        _globalData.ApiController.TakeGlobalLeaderboardPage(LoadingGlobalLeaderboard, Error, levelNum);
        _globalData.ApiController.TakeFriendsLeaderboardPage(LoadingFriendsLeaderboard, Error, levelNum);
    }

    private void LoadingGlobalLeaderboard(LeaderboardPage leaderboardPage)
    {
        globalLeaderboard.SetData(leaderboardPage.Rows);
        globalLeaderboard.SwitchLoadScreenVisible(false);
    }

    private void LoadingFriendsLeaderboard(LeaderboardPage leaderboardPage)
    {
        friendsLeaderboard.SetData(leaderboardPage.Rows);
        globalLeaderboard.SwitchLoadScreenVisible(false);
    }

    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }
    
    public void _LoadLevel()
    {
        _globalData.ApiController.TakeNearWays(OnTakeWaysComplete, Error, _loadLevelNum);
    }

    public void _ShowGlobalLeaderboard() => SwitchLeaderboard(LeaderboardType.Global);
    
    public void _ShowFriendsLeaderboard() => SwitchLeaderboard(LeaderboardType.Friends);

    private void SwitchLeaderboard(LeaderboardType leaderboardType)
    {
        switch (leaderboardType)
        {
            case LeaderboardType.Global:
                globalLeaderboard.SwitchVisible(true);
                friendsLeaderboard.SwitchVisible(false);
                break;
            case LeaderboardType.Friends:
                globalLeaderboard.SwitchVisible(false);
                friendsLeaderboard.SwitchVisible(true);
                break;
            default: 
                throw new Exception("Undefined leaderboard type");
        }
    }
    
    private enum LeaderboardType
    {
        Global = 0,
        Friends = 10
    }
    
    private void OnTakeWaysComplete()
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