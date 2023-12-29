using System;
using System.Collections.Generic;
using SQL_Classes;
using UnityEngine;

namespace UISystem.Other
{
    public class FriendRequestsToMeList : MonoBehaviour
    {
        private RequestsListRow[] _rows;

        private void Awake()
        {
            _rows = gameObject.GetComponentsInChildren<RequestsListRow>();
        }

        public void SetData(List<FriendRequestToMe> someData)
        {
            foreach (var row in _rows)
            {
                row.SwitchVisible(false);
            }
            
            for (int i = 0; i < _rows.Length && i < someData.Count; i++)
            {
                _rows[i].SetPlayerData(someData[i]);
                //_rows[i].OnDeleteFriend += OnDeleteFriend;
                _rows[i].SwitchVisible(true);
            }
        }

        private void OnAcceptRequest(int rowId)
        {
            //Send web api request
        }
        
        private void OnDeAcceptRequest(int rowId)
        {
            //Send web api request
        }
    }
}