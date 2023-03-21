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

public class GhostRecord : MonoBehaviour
{
    [SerializeField] [Range(1,120)] private float recordFrequency;

    public static GhostRecord GhostRecorder;
    [SerializeField] [Range(1,3)] private int levelNum;
    
    public float CurrentFullTime => _currentFullTime;
    private float _currentFullTime;
    public float PreviousFullTime => _previousFullTime;
    private float _previousFullTime = 0;
    
    private Points _points = new Points();
    
    private float _timer = 0;
    private float _currentTime = 0;
    private Transform _playerTransform;
    private bool _record = true;
    
    private void Awake()
    {
        GhostRecorder = this;
    }

    void Start()
    {
        _previousFullTime = NetworkController.Levels[levelNum - 1].time;
        
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

    public void StopRecord()
    {
        _record = false;
        
        
        NetworkController.SavePoints(levelNum, _points);

        
        if (NetworkController.Levels[levelNum - 1].time != 0)
        {
            if(CurrentFullTime < NetworkController.Levels[levelNum-1].time)
                NetworkController.UpdateLevelTime(levelNum, CurrentFullTime);
        }
        else
        {
            NetworkController.UpdateLevelTime(levelNum, CurrentFullTime);
        }
        
    }
}