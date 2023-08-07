using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    [SerializeField] [Range(1,3)] private int levelNum;
    [SerializeField] [Range(1,5)] private int ghostsNumber;
    [SerializeField] private GhostRecord ghostRecord;
    [SerializeField] private GhostReplay ghostReplay;

    public float CurrentFullTime => _currentFullTime;
    private float _currentFullTime;
    public float PreviousFullTime => _previousFullTime;
    private float _previousFullTime = 0;
    
    public int LevelNum => levelNum;
    
    public static GhostSystem This;
    
    private void Awake()
    {
        This = this;
        ghostRecord.OnAwake();
        ghostReplay.OnAwake();
    }
    
    void Start()
    {
        _previousFullTime = NetworkController.Instance.Levels[levelNum - 1].time;
        ghostRecord.OnStart();
        ghostReplay.OnStart();
    }
    
    void Update()
    {
        _currentFullTime += Time.deltaTime;
        ghostRecord.OnUpdate();
        ghostReplay.OnUpdate();
    }

    public void Finish()
    {
        ghostRecord.StopRecord();
        ghostReplay.StopReplay();
    }
}
