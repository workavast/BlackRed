using System.Linq;
using UISystem.Other;
using UnityEngine;
using WEB_API;

namespace UISystem.Screens
{
    public class FriendsScreen : UIScreenBase
    {
        [SerializeField] private FriendList friendList;
        [SerializeField] private FriendRequestsFromMeList requestsFromMeFromMeList;
        [SerializeField] private FriendRequestsToMeList friendRequestsToMeToMeList;

         private GlobalData _globalData => GlobalData.Instance;
         
        private void Awake()
        {
            _globalData.NetworkController.TakeFriends(OnFriendListLoad, null);
            _globalData.NetworkController.TakeFromMeRequests(OnRequestsListLoad, null);
            _globalData.NetworkController.TakeToMeRequests(OnSendRequestsListLoad, null);
        }

        private void OnFriendListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendPairs;
            friendList.SetData(data.ToList());
        }
        
        private void OnRequestsListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendRequestsToMe;
            friendRequestsToMeToMeList.SetData(data.ToList());
        }
        
        private void OnSendRequestsListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendRequestsFromMe;
            requestsFromMeFromMeList.SetData(data.ToList());
        }
    }
}