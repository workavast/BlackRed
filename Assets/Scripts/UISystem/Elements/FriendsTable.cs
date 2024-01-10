using System;
using WebApiConverters;

namespace UISystem.Elements
{
    public class FriendsTable : TableBase<FriendListRow, FriendPair>
    {
        public event Action<int> OnDeleteFriend;

        protected override void OnInit()
        {
            base.OnInit();
            
            foreach (var row in Rows)
                row.OnDeleteFriend += DeleteFriend;
        }
        
        protected override void SetRowData(FriendListRow row, FriendPair newData)
        {
            row.SetData(newData);
            row.SwitchVisible(true);
        }

        private void DeleteFriend(int friendPairId) => OnDeleteFriend?.Invoke(friendPairId);
    }
}