using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQL_Classes;
using UnityEngine.Events;

[Serializable]
public class GhostReplay
{
    [SerializeField] private GameObject playerGhostPrefab;
    private GameObject _playerGhost;
    
    private GhostSystem _ghostSystem;
    
    private bool _replay = true;
    
    private List<Point> _points;
    private int _currentPointNum;
    
    private Point _prevPoint;
    private Point _targetPoint;

    
    public void OnAwake()
    {
        _ghostSystem = GhostSystem.This;
        _points = NetworkController.Instance.PlayerPoints;
        
        if (_points.Count == 0)
            _replay = false;
    }

    public void OnStart()
    {
        if (_replay)
        {
            _playerGhost = GameObject.Instantiate(playerGhostPrefab);
            _prevPoint =  _points[_currentPointNum++];
            _targetPoint = _points[_currentPointNum++];
            
            _playerGhost.transform.position = _prevPoint.Position;
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

                _prevPoint = _points[_currentPointNum - 1];
                _targetPoint = _points[_currentPointNum];

                if (_currentPointNum >= _points.Count)
                    StopReplay();
            }
            
            Vector3 newPosition = Vector3.Lerp(_prevPoint.Position, _targetPoint.Position,
                (_ghostSystem.CurrentFullTime - _prevPoint.time) / (_targetPoint.time - _prevPoint.time));

            _playerGhost.transform.position = newPosition;
        }

    }
    
    public void StopReplay()
    {
        _replay = false;
    }
}
