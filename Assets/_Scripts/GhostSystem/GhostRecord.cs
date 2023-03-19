using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public float x;
    public float y;
    public float z;
    public float time;

    public Point(float X, float Y, float Z, float Time)
    {
        x = X;
        y = Y;
        z = Z;
        time = Time;
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

public class GhostRecord : MonoBehaviour
{
    [SerializeField] [Range(1,120)] private float recordFrequency;

    public static float CurrentFullTime => _currentFullTime;
    private static float _currentFullTime;
    public static float PreviousFullTime => _previousFullTime;
    private static float _previousFullTime = 0;
    
    private static Points _points = new Points();
    
    private float _timer = 0;
    private float _currentTime = 0;
    private Transform _playerTransform;
    private static bool _record = true;
    
    void OnLevelWasLoaded(int level)
    {
        _currentFullTime = 0;
        _previousFullTime = NetworkController.Levels[0].time;
        _record = true;
    }
    
    void Start()
    {
        _previousFullTime = NetworkController.Levels[0].time;
        
        _timer = 60 / recordFrequency;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if (_record)
        {
            _currentFullTime += Time.deltaTime;
            if (_currentTime >= _timer)
            {
                Vector3 currentPos = _playerTransform.position;
                _points.Add(currentPos.x,currentPos.y,currentPos.z, CurrentFullTime);
                
                _currentTime = 0;
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }
    }

    public static void StopRecord()
    {
        _record = false;
        NetworkController.SavePoints(1, _points);
    }
}
