using System;
using SQL_Classes;

namespace UISystem.Elements
{
    public class FriendRequestsToMeTable : TableBase<FriendRequestsToMeRow, FriendRequestToMe>
    {
        public event Action<int> OnAcceptRequest;
        public event Action<int> OnDeAcceptRequest;
        
        protected override void OnInit()
        {
            foreach (var row in Rows)
            {
                row.OnAcceptRequest += AcceptRequest;
                row.OnDeAcceptRequest += DeAcceptRequest;
            }
        }

        protected override void SetRowData(FriendRequestsToMeRow row, FriendRequestToMe newData)
        {
            row.SetData(newData);
            row.SwitchVisible(true);
        }

        private void AcceptRequest(int requestId) => OnAcceptRequest?.Invoke(requestId);

        private void DeAcceptRequest(int requestId) => OnDeAcceptRequest?.Invoke(requestId);
    }
}