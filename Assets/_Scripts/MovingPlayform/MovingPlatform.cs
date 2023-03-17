using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float xOffset;
    private Vector3 _startPos;
    private int _direction = 1;
    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private Vector3 _currentPos;
    
    private void Start()
    {
        _startPos = transform.position;
        _currentPos = _startPos;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (xOffset <= Mathf.Abs(_startPos.x - _currentPos.x))
        {
            _direction *= -1;
        }

        _currentPos +=  new Vector3(speed * _direction * Time.deltaTime, 0, 0);
        
        _rigidbody.MovePosition(_currentPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterController = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _characterController.Move(_rigidbody.velocity * Time.deltaTime);
        }
    }
}
