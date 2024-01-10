using System;
using System.Linq;
using DataStorages;
using GhostSystem;
using Newtonsoft.Json;
using SharedLibrary.Paths;
using SharedLibrary.Requests.LevelsController;
using SharedLibrary.Responses.LevelsController;
using WebApiConverters;

namespace Apis
{
    public class LevelsApi : ApiBase
    {
        protected override string ControllerPath => "Levels";
        
        public LevelsApi(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void RegisterLevelResult(Action onSuccess, Action<string> onError, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost(LevelsControllerPaths.RegisterLevelResult, request,onError, JwtToken);
            if (!res) return;
            
            PlayerDataStorage.RegisterNewLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccess?.Invoke();
        }
        
        public async void UpdateLevelResult(Action onSuccess, Action<string> onError, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost(LevelsControllerPaths.UpdateLevelResult, request, onError, JwtToken);
            if (!res) return;
            
            PlayerDataStorage.UpdateLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccess?.Invoke();
        }

        public async void TakePlayerLevelsData(Action onSuccess, Action<string> onError)
        {
            var res = await TryGet<TakePlayerLevelsDataResponse>
                (LevelsControllerPaths.TakePlayerLevelsData, onError, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.LevelsData
                .Select(l => new PlayerLevelData(l.Num, l.Time, JsonConvert.DeserializeObject<Way>(l.Way)))
                .ToList();
            PlayerDataStorage.SetLevelsDatas(finalRes);
            
            onSuccess?.Invoke();
        }
        
        public async void TakeNearWays(Action onSuccess, Action<string> onError, int levelNum)
        {
            var request = new TakeNearWaysRequest(levelNum);
            
            var res = await TryPost<TakeNearWaysResponse>
                (LevelsControllerPaths.TakeNearWays, request, onError, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.Ways.Select(JsonConvert.DeserializeObject<Way>).ToList();
            CurrentLevelData.LevelNum = levelNum;
            CurrentLevelData.OtherPlayersWays = new SomeWays(finalRes);
            
            onSuccess?.Invoke();
        }
        
        public async void TakeGlobalLeaderboardPage(Action<LeaderboardPage> onSuccess, Action<string> onError, int levelNum)
        {
            var request = new TakeLeaderboardPageRequest(levelNum);

            var res = await TryPost<TakeLeaderboardPageResponse>
                (LevelsControllerPaths.TakeGlobalLeaderboardPage, request, onError, JwtToken);
            if (!res.Item1) return;
            
            onSuccess?.Invoke(new LeaderboardPage(res.Item2.Rows));
        }

        public async void TakeFriendsLeaderboardPage(Action<LeaderboardPage> onSuccess, Action<string> onError, int levelNum)
        {
            var request = new TakeLeaderboardPageRequest(levelNum);

            var res = await TryPost<TakeLeaderboardPageResponse>
                (LevelsControllerPaths.TakeFriendsLeaderboardPage, request, onError, JwtToken);
            if (!res.Item1) return;
            
            onSuccess?.Invoke(new LeaderboardPage(res.Item2.Rows));
        }
    }
}