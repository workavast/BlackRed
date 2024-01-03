using System;
using SQL_Classes;

namespace UISystem.Elements
{
    public class FriendRequestFromMePageRow : TablePageRowBase<FriendRequestFromMe>
    {
        private int _requestId;

        public event Action<int> OnCancelRequest;
        
        public override void SetData(FriendRequestFromMe newData)
        {
            playerName.text = newData.UserName;
            _requestId = newData.Id;        
        }
        
        public void _OnCancelFriend() => OnCancelRequest?.Invoke(_requestId);
    }
}