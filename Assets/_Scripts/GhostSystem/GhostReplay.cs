using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQL_Classes;

[Serializable]
public class GhostReplay
{
    [SerializeField] private int ghostsNumber;
    private Points _points;
    private int _levelNum;
    
    public void OnAwake()
    {
        _levelNum = GhostSystem.This.LevelNum;
    }

    public void OnStart()
    {
        
    }
    
    public void OnUpdate()
    {
        
    }
    
    public void StopReplay()
    {
        
    }
}
