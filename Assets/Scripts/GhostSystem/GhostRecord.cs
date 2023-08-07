using System.Collections.Generic;
using UnityEngine;
using SQL_Classes;
using System;

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
        
        if(_ghostSystem.CurrentFullTime > NetworkController.Instance.Levels[_levelNum - 1].time && NetworkController.Instance.Levels[_levelNum - 1].time > 0 )
            return;
        
        SavePoints();
    }

    private void SavePoints()
    {
        NetworkController.Instance.SaveWay(UpdateLevelTime, Error, _levelNum, _way);
    }
    
    private void UpdateLevelTime()
    {
        NetworkController.Instance.UpdateLevelTime(TakeNearWays, Error, _levelNum, _ghostSystem.CurrentFullTime);
    }
    
    private void TakeNearWays()
    {
        NetworkController.Instance.TakeWays(Completed, Error, _levelNum, NetworkController.Instance.Levels[_levelNum - 1].time);
    }
    
    private void Completed(int levelNum)
    {
        Debug.Log("OK");
    }
    
    private void Error(string errorText)
    {
        UIController.ShowError(errorText);
    }
}