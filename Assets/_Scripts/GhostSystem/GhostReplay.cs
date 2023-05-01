using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQL_Classes;
using UnityEngine.Events;
using Object = UnityEngine.Object;


public class Ghost
{
    private GameObject _ghostPrefab;
    
    private GameObject _ghost;

    private GhostSystem _ghostSystem;

    private bool _replay = true;

    private List<Point> _points;
    private int _currentPointNum;

    private Point _prevPoint;
    private Point _targetPoint;

    public Ghost(GameObject ghostPrefab, List<Point> points)
    {
        _ghostPrefab = ghostPrefab;
        _points = points;
    }

    public void OnAwake()
    {
        _ghostSystem = GhostSystem.This;
    }

    public void OnStart()
    {
        _ghost = Object.Instantiate(_ghostPrefab);
        _prevPoint =  _points[_currentPointNum++];
        _targetPoint = _points[_currentPointNum++];
            
        _ghost.transform.position = _prevPoint.Position;
    }
    
    public void OnUpdate()
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

        _ghost.transform.position = newPosition;
    }
    
    private void StopReplay()
    {
        _replay = false;
    }
}


[Serializable]
public class GhostReplay
{
    [SerializeField] private GameObject playerGhostPrefab;
    [SerializeField] private GameObject otherPlayersGhostsPrefab;
    
    private bool _replay = true;

    private Ghost _playerGhost;
    private List<Ghost> _otherPlayersGhosts = new List<Ghost>();
    
    public void OnAwake()
    {
        if (NetworkController.Instance.PlayerPoints.Count == 0)
        {
            StopReplay();
            return;
        }
        
        _playerGhost = new Ghost(playerGhostPrefab, NetworkController.Instance.PlayerPoints);
        _playerGhost.OnAwake();
        
        List<Way> ways = NetworkController.Instance.OtherWays.ways;

        for (int i = 0; i < ways.Count; i++)
        {
            _otherPlayersGhosts.Add(new Ghost(otherPlayersGhostsPrefab, ways[i].points));
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
