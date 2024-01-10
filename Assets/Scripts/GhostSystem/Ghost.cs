using System.Collections.Generic;
using UnityEngine;

namespace GhostSystem
{
    public class Ghost
    {
        private readonly GameObject _ghostPrefab;
        private readonly List<Point> _points;

        private GameObject _ghost;

        private GhostController _ghostController;

        private bool _replay = true;

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
            _ghostController = GhostController.This;
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
            if (_ghostController.CurrentFullTime >= _points[_currentPointNum].time)
            {
                while (_ghostController.CurrentFullTime >= _points[_currentPointNum].time)
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
                (_ghostController.CurrentFullTime - _currentPoint.time) / (_targetPoint.time - _currentPoint.time));

            _ghost.transform.position = newPosition;
        }
    
        private void StopReplay()
        {
            _replay = false;
        }
    }
}