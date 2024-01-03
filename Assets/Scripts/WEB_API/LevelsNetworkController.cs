using System;
using System.Linq;
using Newtonsoft.Json;
using SharedLibrary.Paths;
using SharedLibrary.Requests.LevelsController;
using SharedLibrary.Responses.LevelsController;
using SQL_Classes;

namespace WEB_API
{
    public class LevelsNetworkController : NetworkControllerBase
    {
        protected override string ControllerPath => LevelsPath;
        private const string LevelsPath = "Levels";
        
        public LevelsNetworkController(PlayerDataStorage playerDataStorage,
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void RegisterLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost
                (LevelsControllerPaths.RegisterLevelResult, request,onErrorDelegate, JwtToken);
            if (!res) return;
            
            PlayerDataStorage.RegisterNewLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void UpdateLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost
                (LevelsControllerPaths.UpdateLevelResult, request, onErrorDelegate, JwtToken);
            if (!res) return;
            
            PlayerDataStorage.UpdateLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }

        public async void TakePlayerLevelsData(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakePlayerLevelsDataResponse>
                (LevelsControllerPaths.TakePlayerLevelsData, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.LevelsData
                .Select(l => new PlayerLevelData(l.Num, l.Time, JsonConvert.DeserializeObject<Way>(l.Way)))
                .ToList();
            PlayerDataStorage.SetLevelsDatas(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeNearWays(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            var request = new TakeNearWaysRequest(levelNum);
            
            var res = await TryPost<TakeNearWaysResponse>
                (LevelsControllerPaths.TakeNearWays, request, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.Ways.Select(JsonConvert.DeserializeObject<Way>).ToList();
            CurrentLevelData.LevelNum = levelNum;
            CurrentLevelData.OtherPlayersWays = new SomeWays(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeGlobalLeaderboardPage
            (Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            var request = new TakeLeaderboardPageRequest(levelNum);

            var res = await TryPost<TakeLeaderboardPageResponse>
                (LevelsControllerPaths.TakeGlobalLeaderboardPage, request, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            onSuccessDelegate?.Invoke(new LeaderboardPage(res.Item2.Rows));
        }

        public async void TakeFriendsLeaderboardPage
            (Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            var request = new TakeLeaderboardPageRequest(levelNum);

            var res = await TryPost<TakeLeaderboardPageResponse>
                (LevelsControllerPaths.TakeFriendsLeaderboardPage, request, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            onSuccessDelegate?.Invoke(new LeaderboardPage(res.Item2.Rows));
        }
    }
}