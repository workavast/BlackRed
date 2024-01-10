using DataStorages;
using TMPro;
using UISystem.Elements;
using UnityEngine;
using WEB_API;

namespace UISystem.Screens
{
    public class FriendsScreen : UIScreenBase
    {
        [SerializeField] private FriendsTable friendsTable;
        [SerializeField] private FriendRequestsFromMeTable friendRequestsFromMeTable;
        [SerializeField] private FriendRequestsToMeTable friendRequestsToMeTable;
        [SerializeField] private TMP_InputField addFriendName;

         private GlobalData _globalData => GlobalData.Instance;

         private void Awake()
         {
             friendsTable.Init();
             friendRequestsFromMeTable.Init();
             friendRequestsToMeTable.Init();
             
             friendsTable.OnDeleteFriend += DeleteFriend;
             friendRequestsFromMeTable.OnCancelRequest += CancelRequest;
             friendRequestsToMeTable.OnAcceptRequest += AcceptRequest;
             friendRequestsToMeTable.OnDeAcceptRequest += DeAcceptRequest;
         }

         private void UpdateInfo()
        {
            friendsTable.SwitchLoadScreenVisible(true);
            friendRequestsFromMeTable.SwitchLoadScreenVisible(true);
            friendRequestsToMeTable.SwitchLoadScreenVisible(true);
            
            _globalData.ApiController.TakeFriends(OnFriendListLoad, null);
            _globalData.ApiController.TakeFromMeRequests(OnRequestsFromMeListLoad, null);
            _globalData.ApiController.TakeToMeRequests(OnFriendRequestsToMeListLoad, null);
        }
        
        private void OnFriendListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendPairs;
            friendsTable.SetData(data);
            friendsTable.SwitchLoadScreenVisible(false);
        }
        
        private void OnFriendRequestsToMeListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendRequestsToMe;
            friendRequestsToMeTable.SetData(data);
            friendRequestsToMeTable.SwitchLoadScreenVisible(false);
        }
        
        private void OnRequestsFromMeListLoad()
        {
            var data = _globalData.FriendsDataStorage.FriendRequestsFromMe;
            friendRequestsFromMeTable.SetData(data);
            friendRequestsFromMeTable.SwitchLoadScreenVisible(false);
        }

        private void DeleteFriend(int friendPairId)
        {
            _globalData.ApiController.DeleteFriend(UpdateInfo, OnError, friendPairId);
        }

        private void AcceptRequest(int requestId)
        {
            _globalData.ApiController.AcceptRequest(UpdateInfo, OnError, requestId);
        }

        private void DeAcceptRequest(int requestId)
        {
            _globalData.ApiController.DeAcceptRequest(UpdateInfo, OnError, requestId);
        }
        
        private void CancelRequest(int requestId)
        {
            _globalData.ApiController.CancelFriendRequest(UpdateInfo, OnError, requestId);
        }
        
        public void _SendFriendRequest()
        {
            if(addFriendName.text == "") return;
            
            var userName = addFriendName.text;
            _globalData.ApiController.SendFriendRequest(UpdateInfo, OnError, userName);
            addFriendName.text = "";
        }

        private void OnError(string errorText) => UIController.ShowError(errorText);
        
        public void _UpdateInfo() => UpdateInfo();
        private void OnEnable() => UpdateInfo();
    }
}