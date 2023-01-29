using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SomeStorage
{
    [SerializeField] protected float maxValue;
    [SerializeField] protected float currentValue;

    public float MaxValue => maxValue;
    public float CurrentValue => currentValue;

    public SomeStorage()
    {
        maxValue = 0;
        currentValue = 0;
    }

    public SomeStorage(float maxValue)
    {
        this.maxValue = maxValue;
    }

    public SomeStorage(float maxValue, float currentValue)
    {
        this.maxValue = maxValue;
        this.currentValue = currentValue;
    }
    
    public void SetMaxValue(float maxValue)
    {
        this.maxValue = maxValue;
        ChangeCurrentValue(0);
    }

    public void SetCurrentValue(float currentValue)
    {
        this.currentValue = currentValue;
        ChangeCurrentValue(0);
    }
    
    public void ChangeCurrentValue(float value)
    {
        currentValue += value;

        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }
}
