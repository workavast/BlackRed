using System;
using SQL_Classes;
using TMPro;
using UnityEngine;

namespace UISystem.Other
{
    public class FriendRequestFromMeRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        private int _requestId;

        public event Action<int> OnDeleteFriend;
        
        public void SetPlayerData(FriendRequestFromMe friendRequestFromMe)
        {
            playerName.text = friendRequestFromMe.UserName;
            _requestId = friendRequestFromMe.Id;
        }
        
        public void SetPlayerData(string newPlayerName, int newRequestId)
        {
            playerName.text = newPlayerName;
            _requestId = newRequestId;
        }
        
        public void _OnDeleteFriend() => OnDeleteFriend?.Invoke(_requestId);

        public void SwitchVisible(bool show) => gameObject.SetActive(show);
    }
}