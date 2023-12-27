using System;
using System.Linq;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SQL_Classes;

namespace WEB_API
{
    public class NetworkController
    {
        private const string PartOfPath = "http://localhost:5159/";

        private readonly PlayerDataStorage _playerDataStorage;
        private readonly CurrentLevelData _currentLevelData;

        public NetworkController(PlayerDataStorage playerDataStorage, CurrentLevelData currentLevelData)
        {
            _playerDataStorage = playerDataStorage;
            _currentLevelData = currentLevelData;
        }
        
        public void Clear()
        {
            
        }
        
        public async void UserRegistration(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            const string fullPath = PartOfPath + "Authentication/Register";
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await WebApi.Post<AuthenticationResponse>(fullPath, req);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            _playerDataStorage.SetMainData(playerName, playerPassword, res.Item3.Toke);
            TakePlayerLevelsData(onSuccessDelegate, onErrorDelegate);
        }
        
        public async void UserLogin(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            const string fullPath = PartOfPath + "Authentication/Login";
            var req = new AuthenticationRequest(playerName, playerPassword);
        
            var res = await WebApi.Post<AuthenticationResponse>(fullPath, req);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            _playerDataStorage.SetMainData(playerName, playerPassword, res.Item3.Toke);
            TakePlayerLevelsData(onSuccessDelegate, onErrorDelegate);
        }
        
        public async void RegisterLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            const string fullPath = PartOfPath + "Level/RegisterLevelResult";
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await WebApi.Post(fullPath, request, _playerDataStorage.JwtToken);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            _playerDataStorage.RegisterNewLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void UpdateLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            const string fullPath = PartOfPath + "Level/UpdateLevelResult";
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await WebApi.Post(fullPath, request, _playerDataStorage.JwtToken);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            _playerDataStorage.UpdateLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }

        private async void TakePlayerLevelsData(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            const string fullPath = PartOfPath + "Level/TakePlayerLevelsData";
        
            var res = await WebApi.Get<TakePlayerLevelsDataResponse>(fullPath, _playerDataStorage.JwtToken);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }
            
            var finalRes = res.Item3.LevelsData
                .Select(l => new PlayerLevelData(l.Num, l.Time, JsonConvert.DeserializeObject<Way>(l.Way)))
                .ToList();
            _playerDataStorage.SetLevelsDatas(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeNearWays(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            const string fullPath = PartOfPath + "Level/TakeNearWays";
            var request = new TakeNearWaysRequest(levelNum);
            
            var res = await WebApi.Post<TakeNearWaysResponse>(fullPath, request, _playerDataStorage.JwtToken);
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            var finalRes = res.Item3.Ways.Select(JsonConvert.DeserializeObject<Way>).ToList();
            _currentLevelData.LevelNum = levelNum;
            _currentLevelData.OtherPlayersWays = new SomeWays(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeLeaderboardPage(Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            const string fullPath = PartOfPath + "Level/TakeLeaderboardPage";
            var request = new TakeLeaderboardPageRequest(levelNum);
            
            var res = await WebApi.Post<TakeLeaderboardPageResponse>(fullPath, request, _playerDataStorage.JwtToken);;
            if (!res.Item1)
            {
                onErrorDelegate?.Invoke(res.Item2);
                return;
            }

            onSuccessDelegate?.Invoke(new LeaderboardPage(res.Item3.Rows));
        }
    }
}