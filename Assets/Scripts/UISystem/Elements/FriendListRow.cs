using System;
using WebApiConverters;

namespace UISystem.Elements
{
    public class FriendListRow : TablePageRowBase<FriendPair>
    {
        private int _friendPairId;

        public event Action<int> OnDeleteFriend;
        
        public override void SetData(FriendPair newData)
        {
            playerName.text = newData.FriendName;
            _friendPairId = newData.Id;
        }

        public void _OnDeleteFriend()
        {
            OnDeleteFriend?.Invoke(_friendPairId);
            SwitchVisible(false);
        }
    }
}