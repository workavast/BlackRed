using System;
using System.Collections.Generic;
using SQL_Classes;
using UnityEngine;

namespace UISystem.Other
{
    public class FriendList : MonoBehaviour
    {
        private FriendListRow[] _rows;

        private void Awake()
        {
            _rows = gameObject.GetComponentsInChildren<FriendListRow>();
        }

        public void SetData(List<FriendPair> someData)
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

        private void OnDeleteFriend(int friendPairId)
        {
            //Send web api request
        }
    }
}