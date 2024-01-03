using System;
using SQL_Classes;

namespace UISystem.Elements
{
    public class FriendRequestsToMeRow : TablePageRowBase<FriendRequestToMe>
    {
        private int _requestId;

        public event Action<int> OnAcceptRequest;
        public event Action<int> OnDeAcceptRequest;
        
        public override void SetData(FriendRequestToMe newData)
        {
            playerName.text = newData.UserName;
            _requestId = newData.Id;
        }

        public void _OnAccept() => OnAcceptRequest?.Invoke(_requestId);
        public void _OnDeAccept() => OnDeAcceptRequest?.Invoke(_requestId);
    }
}