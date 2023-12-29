using System;
using SQL_Classes;
using TMPro;
using UnityEngine;

namespace UISystem.Other
{
    public class RequestsListRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        private int _requestId;

        public event Action<int> OnAcceptRequest;
        public event Action<int> OnDeAcceptRequest;
        
        public void SetPlayerData(FriendRequestToMe friendRequestFromMe)
        {
            playerName.text = friendRequestFromMe.UserName;
            _requestId = friendRequestFromMe.Id;
        }
        
        public void SetPlayerData(string newPlayerName, int newRequestId)
        {
            playerName.text = newPlayerName;
            _requestId = newRequestId;
        }
        
        public void _OnAccept() => OnAcceptRequest?.Invoke(_requestId);
        public void _OnDeAccept() => OnDeAcceptRequest?.Invoke(_requestId);

        public void SwitchVisible(bool show) => gameObject.SetActive(show);
    }
}