using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected SomeStorage healthPoints;
    protected GameObject PlayerGameObject;
    protected Transform PlayerTransform;

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        PlayerGameObject = GameObject.Find("Player");
        PlayerTransform = PlayerGameObject.transform;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        transform.LookAt(PlayerTransform);
    }

    protected virtual void TakeDamage(float damage)
    {
        healthPoints.ChangeCurrentValue(-damage);
    }
    
}
