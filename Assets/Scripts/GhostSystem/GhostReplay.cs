using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SQL_Classes;
using WEB_API;
using Object = UnityEngine.Object;


public class Ghost
{
    private GameObject _ghostPrefab;
    
    private GameObject _ghost;

    private GhostSystem _ghostSystem;

    private bool _replay = true;

    private List<Point> _points;
    private int _currentPointNum;

    private Point _currentPoint;
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
        _currentPoint =  _points[_currentPointNum++];
        _targetPoint = _points[_currentPointNum++];
            
        _ghost.transform.position = _currentPoint.Position();
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

            _currentPoint = _points[_currentPointNum - 1];
            _targetPoint = _points[_currentPointNum];

            if (_currentPointNum >= _points.Count)
                StopReplay();
        }
            
        Vector3 newPosition = Vector3.Lerp(_currentPoint.Position(), _targetPoint.Position(),
            (_ghostSystem.CurrentFullTime - _currentPoint.time) / (_targetPoint.time - _currentPoint.time));

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
