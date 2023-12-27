using UnityEngine;
using SQL_Classes;
using System;
using System.Linq;
using WEB_API;

[Serializable]
public class GhostRecord
{
    [SerializeField] [Range(1,120)] private float recordFrequency;

    private GhostSystem _ghostSystem;
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
        _levelNum = GhostSystem.This.LevelNum;
        _ghostSystem = GhostSystem.This;
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
                    _way.Add(currentPos.x,currentPos.y,currentPos.z, _ghostSystem.CurrentFullTime);
                
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
        if(res != null && _ghostSystem.CurrentFullTime > res.Time)
            return;

        if (_globalData.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == _levelNum) is null)
            RegisterLevelResult();
        else
            UpdateLevelResult();
    }
    
    private void RegisterLevelResult()
    {
        _globalData.NetworkController.RegisterLevelResult(TakeNearWays, Error, _levelNum, _ghostSystem.CurrentFullTime, _way);
    }
    
    private void UpdateLevelResult()
    {
        _globalData.NetworkController.UpdateLevelResult(TakeNearWays, Error, _levelNum, _ghostSystem.CurrentFullTime, _way);
    }
    
    private void TakeNearWays()
    {
        _globalData.NetworkController.TakeNearWays(Completed, Error, _levelNum);
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