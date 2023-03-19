using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecord : MonoBehaviour
{
    private List<Vector3> _points = new List<Vector3>();
    [SerializeField] [Range(1,120)] private float recordFrequency;
    private static float _fullTime;
    public static float FullTime => _fullTime;
    private float _timer = 0;
    private float _currentTime = 0;
    private Transform _playerTransform;
    private static bool _record = true;
    void Start()
    {
        _timer = 60 / recordFrequency;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if (_record)
        {
            _fullTime += Time.deltaTime;
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
