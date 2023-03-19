using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecord : MonoBehaviour
{
    [SerializeField] [Range(1,120)] private float recordFrequency;

    public static float CurrentFullTime => _currentFullTime;
    private static float _currentFullTime;
    public static float PreviousFullTime => _previousFullTime;
    private static float _previousFullTime = 0;
    
    private List<Vector3> _points = new List<Vector3>();

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
                _points.Add(_playerTransform.position);
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
    }
}
