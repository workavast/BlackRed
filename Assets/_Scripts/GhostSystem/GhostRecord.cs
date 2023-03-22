using System.Collections.Generic;
using UnityEngine;
using SQL_Points;

namespace SQL_Points
{
    [System.Serializable]
    public class Point
    {
        public float x;
        public float y;
        public float z;
        public float time;

        public Point(float xCord, float yCord, float zCord, float timeMoment)
        {
            x = xCord;
            y = yCord;
            z = zCord;
            time = timeMoment;
        }
    }

    [System.Serializable]
    public class Points
    {
        public List<Point> points = new List<Point>();

        public void Add(float x, float y, float z, float time)
        {
            points.Add(new Point(x,y,z,time));
        }
    }
}

public class GhostRecord
{
    [SerializeField] [Range(1,120)] private float recordFrequency;

    private GhostSystem _ghostSystem;
    private int _levelNum;
    private float _timer = 0;
    private float _currentTime = 0;
    private Transform _playerTransform;
    private Points _points = new Points();
    private bool _record = true;
    
    public void OnAwake()
    {
        _levelNum = GhostSystem.This.LevelNum;
        _ghostSystem = GhostSystem.This;
    }

    public void OnStart()
    {
        _timer = 60 / recordFrequency;
        _playerTransform = Player.This.transform;
    }
    
    public void OnUpdate()
    {
        if (_record)
        {
            if (_currentTime >= _timer)
            {
                Vector3 currentPos = _playerTransform.position;
                _points.Add(currentPos.x,currentPos.y,currentPos.z, _ghostSystem.CurrentFullTime);
                
                _currentTime = 0;
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
        
        NetworkController.SavePoints(_levelNum, _points);
        if (NetworkController.Levels[_levelNum - 1].time != 0)
        {
            if(_ghostSystem.CurrentFullTime < NetworkController.Levels[_levelNum-1].time)
                NetworkController.UpdateLevelTime(_levelNum, _ghostSystem.CurrentFullTime);
        }
        else
        {
            NetworkController.UpdateLevelTime(_levelNum, _ghostSystem.CurrentFullTime);
        }
    }
}