using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float xOffset;
    private Vector3 startPos;
    private int direction = 1;
    
    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (xOffset <= Mathf.Abs(startPos.x - transform.position.x))
        {
            direction *= -1;
        }

        Vector3 newPos = new Vector3(speed * direction * Time.deltaTime, 0, 0);
        transform.Translate(newPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
