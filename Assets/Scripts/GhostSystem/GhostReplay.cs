using System;
using System.Collections.Generic;
using System.Linq;
using DataStorages;
using UnityEngine;

namespace GhostSystem
{
    [Serializable]
    public class GhostReplay
    {
        [SerializeField] private GameObject playerGhostPrefab;
        [SerializeField] private GameObject otherPlayersGhostsPrefab;
    
        private bool _replay = true;

        private Ghost _playerGhost;
        private List<Ghost> _otherPlayersGhosts = new List<Ghost>();
        private GlobalData _globalData => GlobalData.Instance;

        public void OnAwake()
        {
            Debug.Log(_globalData.PlayerDataStorage.Levels.Count);
            Debug.Log(_globalData.CurrentLevelData.LevelNum);
        
            var res = _globalData.PlayerDataStorage.Levels.FirstOrDefault(l =>
                l.Num == _globalData.CurrentLevelData.LevelNum);
            if (res is null)
            {
                Debug.Log("is null");
                StopReplay();
                return;
            }
        
            _playerGhost = new Ghost(playerGhostPrefab, res.Way.points);
            Debug.Log("new player ghost");
            _playerGhost.OnAwake();
        
            SomeWays someWays = _globalData.CurrentLevelData.OtherPlayersWays;
            for (int i = 0; i < someWays.Ways.Count; i++)
            {
                _otherPlayersGhosts.Add(new Ghost(otherPlayersGhostsPrefab, someWays.Ways[i].points));
                Debug.Log("new other player ghost");
                _otherPlayersGhosts[i].OnAwake();
            }
        }

        public void OnStart()
        {
            if (_replay)
            {
                _playerGhost.OnStart();
                foreach (var ghost in _otherPlayersGhosts)
                {
                    ghost.OnStart();
                }
            }
        }
    
        public void OnUpdate()
        {
            if (_replay)
            {
                _playerGhost.OnUpdate();
                foreach (var ghost in _otherPlayersGhosts)
                {
                    ghost.OnUpdate();
                }
            }
        }
    
        public void StopReplay()
        {
            _replay = false;
        }
    }
}