using System;
using System.Linq;
using DataStorages;
using UnityEngine;

namespace GhostSystem
{
    [Serializable]
    public class GhostRecord
    {
        [SerializeField] [Range(1,120)] private float recordFrequency;

        private GhostController _ghostController;
        private int _levelNum;
        private float _timer = 0;
        private float _currentTime = 0;
        private Transform _ghostRecordPoint;
        private Vector3 _prevPlayerPosition;
        private Vector3 _prevDirection;
        private Way _way = new Way();
        private bool _record = true;

        private GlobalData _globalData => GlobalData.Instance;
    
        public void OnAwake()
        {
            _levelNum = GhostController.This.LevelNum;
            _ghostController = GhostController.This;
        }

        public void OnStart()
        {
            _timer = 1 / recordFrequency;
            _ghostRecordPoint = Player.This.GhostRecordPoint;
            _prevPlayerPosition = _ghostRecordPoint.position;
            _prevDirection = (_ghostRecordPoint.position - _prevPlayerPosition).normalized;
        }
    
        public void OnUpdate()
        {
            if (_record)
            {
                if (_currentTime >= _timer)
                {
                    Vector3 direction = (_ghostRecordPoint.position - _prevPlayerPosition).normalized;

                    if (_prevDirection != direction)
                    {
                        Vector3 currentPos = _ghostRecordPoint.position;
                        _way.Add(currentPos.x,currentPos.y,currentPos.z, _ghostController.CurrentFullTime);
                
                        _currentTime = 0;
                    
                        _prevPlayerPosition = _ghostRecordPoint.position;

                    }

                    _prevDirection = direction.normalized;
                }
                else
                {
                    _currentTime += Time.deltaTime;
                }
            
            }
        }

        public void StopRecord()
        {
            _record = false;

            var res = GlobalData.Instance.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == _levelNum);
            if(res != null && _ghostController.CurrentFullTime > res.Time)
                return;

            if (_globalData.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == _levelNum) is null)
                RegisterLevelResult();
            else
                UpdateLevelResult();
        }
    
        private void RegisterLevelResult()
        {
            _globalData.ApiController.RegisterLevelResult(TakeNearWays, Error, _levelNum, _ghostController.CurrentFullTime, _way);
        }
    
        private void UpdateLevelResult()
        {
            _globalData.ApiController.UpdateLevelResult(TakeNearWays, Error, _levelNum, _ghostController.CurrentFullTime, _way);
        }
    
        private void TakeNearWays()
        {
            _globalData.ApiController.TakeNearWays(Completed, Error, _levelNum);
        }
    
        private void Completed()
        {
            Debug.Log("OK");
        }
    
        private void Error(string errorText)
        {
            UIController.ShowError(errorText);
        }
    }
}