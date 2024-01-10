using System;
using System.Linq;
using DataStorages;
using SharedLibrary.Paths;
using SharedLibrary.Requests.FriendsController;
using SharedLibrary.Responses.FriendsController;
using WebApiConverters;

namespace Apis
{
    public class FriendsApi : ApiBase
    {
        protected override string ControllerPath => "Friends";

        public FriendsApi(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void SendFriendRequest(Action onSuccess, Action<string> onError, string userName)
        {
            var data = new SendFriendReqRequest(userName);

            var res = await TryPost(FriendsControllerPaths.SendFriendReq, data, onError, JwtToken);
            if(!res) return;
            
            onSuccess?.Invoke();
        }
        
        public async void AcceptRequest(Action onSuccess, Action<string> onError, int requestId)
        {
            var data = new AcceptFriendReqRequest(requestId);

            var res = await TryPost(FriendsControllerPaths.AcceptFriendReq, data, onError, JwtToken);
            if(!res) return;
            
            onSuccess?.Invoke();
        }
        
        public async void DeAcceptRequest(Action onSuccess, Action<string> onError, int requestId)
        {
            var data = new DeAcceptFriendReqRequest(requestId);

            var res = await TryPost(FriendsControllerPaths.DeAcceptFriendReq, data, onError, JwtToken);
            if(!res) return;
            
            onSuccess?.Invoke();
        }
        
        public async void CancelFriendRequest(Action onSuccess, Action<string> onError, int requestId)
        {
            var data = new CancelFriendReqRequest(requestId);

            var res = await TryPost(FriendsControllerPaths.CancelFriendReq, data, onError, JwtToken);
            if(!res) return;
            
            onSuccess?.Invoke();
        }
        
        public async void DeleteFriend(Action onSuccess, Action<string> onError, int friendPairId)
        {
            var data = new DeleteFriendRequest(friendPairId);

            var res = await TryPost(FriendsControllerPaths.DeleteFriend, data, onError, JwtToken);
            if(!res) return;
            
            onSuccess?.Invoke();
        }
        
        public async void TakeFriends(Action onSuccess, Action<string> onError)
        {
            var res = await TryGet<TakeFriendsResponse>
                (FriendsControllerPaths.TakeFriends, onError, JwtToken);
            if(!res.Item1) return;
            
            FriendsDataStorage.SetFriendPairsData
                (res.Item2.FriendPairs.Select(fp => new FriendPair(fp)).ToList());
            
            onSuccess?.Invoke();
        }
        
        public async void TakeFromMeRequests(Action onSuccess, Action<string> onError)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsControllerPaths.TakeFromMeRequests, onError, JwtToken);
            if(!res.Item1) return;

            FriendsDataStorage.SetFriendRequestsFromMe
                (res.Item2.FriendRequests.Select(fr => new FriendRequestFromMe(fr)).ToList());
            
            onSuccess?.Invoke();
        }
        
        public async void TakeToMeRequests(Action onSuccess, Action<string> onError)
        {
            var res = await TryGet<TakeFriendReqsResponse>
                (FriendsControllerPaths.TakeToMeRequests, onError, JwtToken);
            if(!res.Item1) return;
            
            FriendsDataStorage.SetFriendRequestsToMe
                (res.Item2.FriendRequests.Select(fr => new FriendRequestToMe(fr)).ToList());
            
            onSuccess?.Invoke();
        }
    }
}