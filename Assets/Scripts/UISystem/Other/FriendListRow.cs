using System;
using SQL_Classes;
using TMPro;
using UnityEngine;

namespace UISystem.Other
{
    public class FriendListRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        
        private int _friendPairId;

        public event Action<int> OnDeleteFriend;
        
        public void SetPlayerData(FriendPair friendPair)
        {
            playerName.text = friendPair.FriendName;
            _friendPairId = friendPair.Id;
        }
            
        public void SetPlayerData(string newPlayerName, int newFriendPairId)
        {
            playerName.text = newPlayerName;
            _friendPairId = newFriendPairId;
        }
        
        public void _OnDeleteFriend()
        {
            OnDeleteFriend?.Invoke(_friendPairId);
            SwitchVisible(false);
        }

        public void SwitchVisible(bool show) => gameObject.SetActive(show);
    }
}