using System;
using System.Linq;
using SharedLibrary.Paths;
using SharedLibrary.Requests.FriendsController;
using SharedLibrary.Responses.FriendsController;
using SQL_Classes;

namespace WEB_API
{
    public class FriendsNetworkController : NetworkControllerBase
    {
        protected override string ControllerPath => FriendsPath;
        private const string FriendsPath = "Friends";

        public FriendsNetworkController(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void SendFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, string userName)
        {
            var data = new SendFriendReqRequest(userName);

            var res = await TryPost
                (FriendsControllerPaths.SendFriendReq, data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void AcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new AcceptFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsControllerPaths.AcceptFriendReq, data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void DeAcceptRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new DeAcceptFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsControllerPaths.DeAcceptFriendReq, data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void CancelFriendRequest(Action onSuccessDelegate, Action<string> onErrorDelegate, int requestId)
        {
            var data = new CancelFriendReqRequest(requestId);

            var res = await TryPost
                (FriendsControllerPaths.CancelFriendReq, data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void DeleteFriend(Action onSuccessDelegate, Action<string> onErrorDelegate, int friendPairId)
        {
            var data = new DeleteFriendRequest(friendPairId);

            var res = await TryPost
                (FriendsControllerPaths.DeleteFriend, data, onErrorDelegate, JwtToken);
            if(!res) return;
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeFriends(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendsResponse>
                (FriendsControllerPaths.TakeFriends, onErrorDelegate, JwtToken);
            if(!res.Item1) return;
            
            FriendsDataStorage.SetFriendPairsData
                (res.Item2.FriendPairs.Select(fp => new FriendPair(fp)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeFromMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsControllerPaths.TakeFromMeRequests, onErrorDelegate, JwtToken);
            if(!res.Item1) return;

            FriendsDataStorage.SetFriendRequestsFromMe
                (res.Item2.FriendRequests.Select(fr => new FriendRequestFromMe(fr)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void TakeToMeRequests(Action onSuccessDelegate, Action<string> onErrorDelegate)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsControllerPaths.TakeToMeRequests, onErrorDelegate, JwtToken);
            if(!res.Item1) return;
            
            FriendsDataStorage.SetFriendRequestsToMe
                (res.Item2.FriendRequests.Select(fr => new FriendRequestToMe(fr)).ToList());
            
            onSuccessDelegate?.Invoke();
        }
    }
}