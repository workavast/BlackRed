using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQL_Classes;
using UnityEngine.Events;

[Serializable]
public class GhostReplay
{
    [SerializeField] private int ghostsNumber;
    [SerializeField] private GameObject playerGhostPrefab;
    private GameObject _playerGhost;
    private List<Point> _points;
    private int _levelNum;
    private int _currentPointNum;
    private GhostSystem _ghostSystem;
    private bool _replay = true;
    
    private Point _currentPoint;
    private Point _targetPoint;
    private Point _prevTargetPoint;
    
    
    public void OnAwake()
    {
        _ghostSystem = GhostSystem.This;
        _levelNum = GhostSystem.This.LevelNum;
        _points = NetworkController.PlayerPoints;
        
        if (_points.Count == 0)
            _replay = false;
    }

    public void OnStart()
    {
        if (_replay)
        {
            _playerGhost = GameObject.Instantiate(playerGhostPrefab);
            _prevTargetPoint =  _points[_currentPointNum++];
            _currentPoint = _prevTargetPoint;
            _targetPoint = _points[_currentPointNum++];
            
            _playerGhost.transform.position = _currentPoint.TakePos();
        }
    }
    
    public void OnUpdate()
    {
        if (_replay)
        {
            if (_ghostSystem.CurrentFullTime >= _points[_currentPointNum].time)
            {
                while (_ghostSystem.CurrentFullTime >= _points[_currentPointNum].time)
                {
                    _currentPointNum++;
                    
                    if (_currentPointNum >= _points.Count - 1)
                    {
                        _currentPointNum = _points.Count - 1;
                        break;
                    }
                }

                _prevTargetPoint = _points[_currentPointNum - 1];
                _targetPoint = _points[_currentPointNum];

                if (_currentPointNum >= _points.Count)
                    _replay = false;
            }
            
            Vector3 pos = Vector3.Lerp(_prevTargetPoint.TakePos(), _targetPoint.TakePos(),
                ((_targetPoint.time - _prevTargetPoint.time) - (_targetPoint.time - _ghostSystem.CurrentFullTime)) / (_targetPoint.time - _prevTargetPoint.time));

            _playerGhost.transform.position = pos;
        }
        
        // if (_replay)
        // {
        //     if (_ghostSystem.CurrentFullTime >= _points[_currentPointNum].time)
        //     {
        //         while (true)
        //         {
        //             if (_ghostSystem.CurrentFullTime >= _points[_currentPointNum].time)
        //             {
        //                 _currentPointNum++;
        //                 if (_currentPointNum >= _points.Count - 1)
        //                 {
        //                     _currentPointNum = _points.Count - 1;
        //                     break;
        //                 }
        //             }
        //             else
        //             {
        //                 break;
        //             }
        //         }
        //
        //         _prevTargetPoint = _points[_currentPointNum - 1];
        //         _targetPoint = _points[_currentPointNum];
        //
        //         if (_currentPointNum >= _points.Count)
        //             _replay = false;
        //     }
        //     
        //     Vector3 pos = Vector3.Lerp(_prevTargetPoint.TakePos(), _targetPoint.TakePos(),
        //         ((_targetPoint.time - _prevTargetPoint.time) - (_targetPoint.time - _ghostSystem.CurrentFullTime)) / (_targetPoint.time - _prevTargetPoint.time));
        //
        //     _playerGhost.transform.position = pos;
        // }
    }
    
    public void StopReplay()
    {
        _replay = false;
    }
}
