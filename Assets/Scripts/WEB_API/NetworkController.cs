using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLibrary.Requests.AuthenticationController;
using SharedLibrary.Requests.FriendsController;
using SharedLibrary.Requests.LevelsController;
using SharedLibrary.Responses.AuthenticationController;
using SharedLibrary.Responses.FriendsController;
using SharedLibrary.Responses.LevelsController;
using SQL_Classes;

namespace WEB_API
{
    public class NetworkController
    {
        private const string MainPartOfPath = "http://localhost:5159";
        private const string AuthenticationPath = "Authentication";
        private const string LevelsPath = "Levels";
        private const string FriendsPath = "Friends";

        private readonly PlayerDataStorage _playerDataStorage;
        private readonly CurrentLevelData _currentLevelData;
        private readonly FriendsDataStorage _friendsDataStorage;
        
        private string JwtToken => _playerDataStorage.JwtToken;
        
        public NetworkController(PlayerDataStorage playerDataStorage, CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage)
        {
            _playerDataStorage = playerDataStorage;
            _currentLevelData = currentLevelData;
            _friendsDataStorage = friendsDataStorage;
        }
        
        public void Clear()
        {
            
        }
        
        public async void UserRegistration(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationPath, "Register", req, onErrorDelegate);
            if(!res.Item1) return;
            
            _playerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            TakePlayerLevelsData(onSuccessDelegate, onErrorDelegate);
        }
        
        public async void UserLogin(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationPath, "Login", req, onErrorDelegate);
            if (!res.Item1) return;

            _playerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            TakePlayerLevelsData(onSuccessDelegate, onErrorDelegate);
        }
        
        public async void RegisterLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost
                (LevelsPath, "RegisterLevelResult", request,onErrorDelegate, JwtToken);
            if (!res) return;
            
            _playerDataStorage.RegisterNewLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void UpdateLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
        {
            var jsonWay = JsonConvert.SerializeObject(way);
            var request = new LevelChangeRequest(levelNum, time, jsonWay);
            
            var res = await TryPost
                (LevelsPath,"UpdateLevelResult", request, onErrorDelegate, JwtToken);
            if (!res) return;
            
            _playerDataStorage.UpdateLevelResult(new PlayerLevelData(levelNum, time , way));
            
            onSuccessDelegate?.Invoke();
        }

        private async void TakePlayerLevelsData(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakePlayerLevelsDataResponse>
                (LevelsPath, "TakePlayerLevelsData", onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.LevelsData
                .Select(l => new PlayerLevelData(l.Num, l.Time, JsonConvert.DeserializeObject<Way>(l.Way)))
                .ToList();
            _playerDataStorage.SetLevelsDatas(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeNearWays(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            var request = new TakeNearWaysRequest(levelNum);
            
            var res = await TryPost<TakeNearWaysResponse>
                (LevelsPath, "TakeNearWays", request, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            var finalRes = res.Item2.Ways.Select(JsonConvert.DeserializeObject<Way>).ToList();
            _currentLevelData.LevelNum = levelNum;
            _currentLevelData.OtherPlayersWays = new SomeWays(finalRes);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeLeaderboardPage(Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
        {
            var request = new TakeLeaderboardPageRequest(levelNum);

            var res = await TryPost<TakeLeaderboardPageResponse>
                (LevelsPath, "TakeLeaderboardPage", request, onErrorDelegate, JwtToken);
            if (!res.Item1) return;
            
            onSuccessDelegate?.Invoke(new LeaderboardPage(res.Item2.Rows));
        }

        public async void SendFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, string userName)
        {
            var data = new SendFriendReqRequest(userName);

            var res = await TryPost
                (FriendsPath, "SendFriendReq", data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void AcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new AcceptFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsPath, "AcceptFriendReq", data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void DeAcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new DeAcceptFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsPath, "DeAcceptFriendReq", data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void CancelFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new CancelFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsPath, "CancelFriendReq", data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void DeleteFriend(Action onSuccessDelegate, Action<string> onErrorDelegate, int friendPairId)
        {
            var data = new DeleteFriendRequest(friendPairId);

            var res = await TryPost
                (FriendsPath, "DeleteFriend", data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeFriends(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendsResponse>
                (FriendsPath, "TakeFriends", onErrorDelegate, JwtToken);
            if(!res.Item1) return;
            
            _friendsDataStorage.SetFriendPairsData(res.Item2.FriendPairs.Select(fp => new FriendPair(fp)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeFromMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsPath, "TakeFromMeRequests", onErrorDelegate, JwtToken);
            if(!res.Item1) return;

            _friendsDataStorage.SetFriendRequestsFromMe(res.Item2.FriendRequests.Select(fr => new FriendRequestFromMe(fr)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeToMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsPath, "TakeToMeRequests", onErrorDelegate, JwtToken);
            if(!res.Item1) return;
            
            _friendsDataStorage.SetFriendRequestsToMe(res.Item2.FriendRequests.Select(fr => new FriendRequestToMe(fr)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
        
        
        public static async Task<(bool, T)> TryGet<T>(string controllerPath, string methodPath,
            Action<string> onErrorDelegate, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{controllerPath}/{methodPath}";
            
            var res = await WebApi.Get<T>(fullPath, jwt);
            if (!res.Item1)
                onErrorDelegate?.Invoke(res.Item2);

            return (res.Item1, res.Item3);
        }
        
        public static async Task<(bool, T)> TryPost<T>(string controllerPath, string methodPath, object data, 
            Action<string> onErrorDelegate, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{controllerPath}/{methodPath}";
            
            var res = await WebApi.Post<T>(fullPath, data, jwt);
            if (!res.Item1)
                onErrorDelegate?.Invoke(res.Item2);

            return (res.Item1, res.Item3);
        }
        
        public static async Task<bool> TryPost(string controllerPath, string methodPath, object data, 
            Action<string> onErrorDelegate, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{controllerPath}/{methodPath}";
            
            var res = await WebApi.Post(fullPath, data, jwt);
            if (!res.Item1)
                onErrorDelegate?.Invoke(res.Item2);

            return res.Item1;
        }
    }
}