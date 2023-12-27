using System.Linq;
using UnityEngine;
using WEB_API;

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
        Debug.Log("Ghost system");
        This = this;
        ghostRecord.OnAwake();
        ghostReplay.OnAwake();
    }
    
    void Start()
    {
        var res = GlobalData.Instance.PlayerDataStorage.Levels.FirstOrDefault(l => l.Num == levelNum);
        if(res != null)
            _previousFullTime = res.Time;
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
