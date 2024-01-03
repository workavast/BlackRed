using System;
using SQL_Classes;

namespace UISystem.Elements
{
    public class FriendRequestsFromMeTable : TableBase<FriendRequestFromMePageRow, FriendRequestFromMe>
    {
        public event Action<int> OnCancelRequest;

        protected override void OnInit()
        {
            foreach (var row in Rows)
                row.OnCancelRequest += CancelRequest;
        }

        protected override void SetRowData(FriendRequestFromMePageRow row, FriendRequestFromMe newData)
        {
            row.SetData(newData);
            row.SwitchVisible(true);
        }

        private void CancelRequest(int requestId) => OnCancelRequest?.Invoke(requestId);
    }
}