using System;
using DataStorages;
using GhostSystem;
using WebApiConverters;

namespace Apis
{
    public class ApiController
    {
        private readonly AuthenticationApi _authenticationApi;
        private readonly LevelsApi _levelsApi;
        private readonly FriendsApi _friendsApi;
        
        public ApiController(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage)
        {
            _authenticationApi = new AuthenticationApi(playerDataStorage, currentLevelData, friendsDataStorage);
            _levelsApi = new LevelsApi(playerDataStorage, currentLevelData, friendsDataStorage);
            _friendsApi = new FriendsApi(playerDataStorage, currentLevelData, friendsDataStorage);
        }
        
        public void UserRegistration(Action onSuccess, Action<string> onError, string playerName, string playerPassword)
            => _authenticationApi.UserRegistration(onSuccess, onError, playerName, playerPassword);
        
        public void UserLogin(Action onSuccess, Action<string> onError, string playerName, string playerPassword)
            => _authenticationApi.UserLogin(onSuccess, onError, playerName, playerPassword);
        
        
        public void RegisterLevelResult(Action onSuccess, Action<string> onError, int levelNum, float time, Way way)
            => _levelsApi.RegisterLevelResult(onSuccess, onError, levelNum, time, way);
        
        public void UpdateLevelResult(Action onSuccess, Action<string> onError, int levelNum, float time, Way way)
            => _levelsApi.UpdateLevelResult(onSuccess, onError, levelNum, time, way);
        
        public void TakePlayerLevelsData(Action onSuccess, Action<string> onError)
            => _levelsApi.TakePlayerLevelsData(onSuccess, onError);
        
        public void TakeNearWays(Action onSuccess, Action<string> onError, int levelNum)
            => _levelsApi.TakeNearWays(onSuccess, onError, levelNum);
        
        public void TakeGlobalLeaderboardPage(Action<LeaderboardPage> onSuccess, Action<string> onError, int levelNum)
            => _levelsApi.TakeGlobalLeaderboardPage(onSuccess, onError, levelNum);
        
        public void TakeFriendsLeaderboardPage(Action<LeaderboardPage> onSuccess, Action<string> onError, int levelNum)
            => _levelsApi.TakeFriendsLeaderboardPage(onSuccess, onError, levelNum);
        
        
        public void SendFriendRequest(Action onSuccess, Action<string> onError, string userName)
            => _friendsApi.SendFriendRequest(onSuccess, onError, userName);
        
        public void AcceptRequest(Action onSuccess, Action<string> onError, int requestId)
            => _friendsApi.AcceptRequest(onSuccess, onError, requestId);
        
        public void DeAcceptRequest(Action onSuccess, Action<string> onError, int requestId)
            => _friendsApi.DeAcceptRequest(onSuccess, onError, requestId);
        
        public void CancelFriendRequest(Action onSuccess, Action<string> onError, int requestId)
            => _friendsApi.CancelFriendRequest(onSuccess, onError, requestId);
        
        public void DeleteFriend(Action onSuccess, Action<string> onError, int friendPairId)
            => _friendsApi.DeleteFriend(onSuccess, onError, friendPairId);
        
        public void TakeFriends(Action onSuccess, Action<string> onError)
            => _friendsApi.TakeFriends(onSuccess, onError);
        
        public void TakeFromMeRequests(Action onSuccess, Action<string> onError)
            => _friendsApi.TakeFromMeRequests(onSuccess, onError);
        
        public void TakeToMeRequests(Action onSuccess, Action<string> onError)
            => _friendsApi.TakeToMeRequests(onSuccess, onError);
    }
}