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
    private GameObject playerGhost;
    private List<Point> _points;
    private int _levelNum;
    private int _currentPoint;
    private GhostSystem _ghostSystem;
    private bool _replay = true;
    
    public void OnAwake()
    {
        _ghostSystem = GhostSystem.This;
        _levelNum = GhostSystem.This.LevelNum;
        _points = NetworkController.PlayerPoints;
    }

    public void OnStart()
    {
        playerGhost = GameObject.Instantiate(playerGhostPrefab);
    }
    
    public void OnUpdate()
    {
        if (_replay)
        {
            if (_ghostSystem.CurrentFullTime >= _points[_currentPoint].time)
            {
                playerGhost.transform.position = _points[_currentPoint].TakePos();
                _currentPoint++;
                
                if (_currentPoint >= _points.Count)
                    _replay = false;
            }
        }
    }
    
    public void StopReplay()
    {
        _replay = false;
    }
}
