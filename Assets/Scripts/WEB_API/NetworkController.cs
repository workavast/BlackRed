using System;
using SQL_Classes;

namespace WEB_API
{
    public class NetworkController
    {
        private readonly AuthenticationNetworkController _authenticationNetworkController;
        private readonly LevelsNetworkController _levelsNetworkController;
        private readonly FriendsNetworkController _friendsNetworkController;
        
        public NetworkController(PlayerDataStorage playerDataStorage, CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage)
        {
            _authenticationNetworkController =
                new AuthenticationNetworkController(playerDataStorage, currentLevelData, friendsDataStorage);
            _levelsNetworkController =
                new LevelsNetworkController(playerDataStorage, currentLevelData, friendsDataStorage);
            _friendsNetworkController =
                new FriendsNetworkController(playerDataStorage, currentLevelData, friendsDataStorage);
        }
        
        public void UserRegistration(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
            => _authenticationNetworkController.UserRegistration(onSuccessDelegate, onErrorDelegate, playerName, playerPassword);
        
        public void UserLogin(Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
            => _authenticationNetworkController.UserLogin(onSuccessDelegate, onErrorDelegate, playerName, playerPassword);
        
        
        public void RegisterLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
            => _levelsNetworkController.RegisterLevelResult(onSuccessDelegate, onErrorDelegate, levelNum, time, way);
        
        public void UpdateLevelResult(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum, float time, Way way)
            => _levelsNetworkController.UpdateLevelResult(onSuccessDelegate, onErrorDelegate, levelNum, time, way);
        
        public void TakePlayerLevelsData(Action onSuccessDelegate, Action<string> onErrorDelegate)
            => _levelsNetworkController.TakePlayerLevelsData(onSuccessDelegate, onErrorDelegate);
        
        public void TakeNearWays(Action onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
            => _levelsNetworkController.TakeNearWays(onSuccessDelegate, onErrorDelegate, levelNum);
        
        public void TakeGlobalLeaderboardPage(Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
            => _levelsNetworkController.TakeGlobalLeaderboardPage(onSuccessDelegate, onErrorDelegate, levelNum);
        
        public void TakeFriendsLeaderboardPage(Action<LeaderboardPage> onSuccessDelegate, Action<string> onErrorDelegate, int levelNum)
            => _levelsNetworkController.TakeFriendsLeaderboardPage(onSuccessDelegate, onErrorDelegate, levelNum);
        
        
        public void SendFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, string userName)
            => _friendsNetworkController.SendFriendRequest(onSuccessDelegate, onErrorDelegate, userName);
        
        public void AcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
            => _friendsNetworkController.AcceptRequest(onSuccessDelegate, onErrorDelegate, requestId);
        
        public void DeAcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
            => _friendsNetworkController.DeAcceptRequest(onSuccessDelegate, onErrorDelegate, requestId);
        
        public void CancelFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
            => _friendsNetworkController.CancelFriendRequest(onSuccessDelegate, onErrorDelegate, requestId);
        
        public void DeleteFriend(Action onSuccessDelegate, Action<string> onErrorDelegate, int friendPairId)
            => _friendsNetworkController.DeleteFriend(onSuccessDelegate, onErrorDelegate, friendPairId);
        
        public void TakeFriends(Action onSuccessDelegate, Action<string> onErrorDelegate)
            => _friendsNetworkController.TakeFriends(onSuccessDelegate, onErrorDelegate);
        
        public void TakeFromMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
            => _friendsNetworkController.TakeFromMeRequests(onSuccessDelegate, onErrorDelegate);
        
        public void TakeToMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
            => _friendsNetworkController.TakeToMeRequests(onSuccessDelegate, onErrorDelegate);
    }
}