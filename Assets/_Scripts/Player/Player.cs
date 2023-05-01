using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
struct ShowGizmos
{
    [SerializeField] public bool groundSphere;
    [SerializeField] public bool ceilingSphere;
    [SerializeField] public bool slopeSphere;
    [SerializeField] public bool stateGizmos;
}

[Serializable]
struct GroundCheckSphere{
    [Tooltip("If the character is grounded or not")]
    public bool onGround;
    [Tooltip("Offset of the sphere that check ground, y axis")]
    public float offset;
    [Tooltip("Sphere radius that check ground")]
    public float radius;
    [Tooltip("What layers used as ground")]
    public LayerMask groundsLayers;
}
[Serializable]
struct CeilingCheckSphere
{
    [Tooltip("If the character is ceiling or not")]
    public bool onCeiling;
    [Tooltip("Offset of the sphere that check ceiling, y axis")]
    public float offset;
    [Tooltip("Sphere radius that check ceiling")]
    public float radius;
    [Tooltip("What layers used as ceiling")]
    public LayerMask ceilingLayers;
}
[Serializable]
struct SlopeCheckSphere
{
    [Tooltip("If the character is sliding or not")] 
    public bool onSlope;
    [Tooltip("Length of the sphereCast that check sliding")] 
    public float length;
    [Tooltip("Offset of the sphere that check sliding, y axis")]
    public float offset;
    [Tooltip("Radius of the sphereCast that check sliding")] 
    public float radius;
    [Tooltip("What layers used as ground")]
    public LayerMask slopeLayers;
    [Tooltip("Minimal clamp for sliding")] 
    public float minSlopeClamp;    
    [Tooltip("Maximal clamp for sliding")] 
    public float maxSlopeClamp;
}

public enum PlayerState
{
    MainMove,
    Jump,
    Fall,
    SlopeMoving,
    Sliding,
    Crouch
}

public class Player : MonoBehaviour, ICastSphereTake
{
    [Header("Gizmos")] 
    [SerializeField] private ShowGizmos showGizmos;

    [Header("Checks")] 
    [SerializeField] private GroundCheckSphere groundCheckSphere;
    [SerializeField] private CeilingCheckSphere ceilingCheckSphere;
    [SerializeField] private SlopeCheckSphere slopeCheckSphere;
    public bool OnGround => groundCheckSphere.onGround;
    public bool OnCeiling => ceilingCheckSphere.onCeiling;
    public bool OnSlope => slopeCheckSphere.onSlope;
    
    [Header("States")]
    [SerializeField] private PlayerState currentStateName;
    [SerializeField] private MainMove mainMove = new MainMove();
    [SerializeField] private Jump jump = new Jump();
    [SerializeField] private Fall fall = new Fall();
    [SerializeField] private SlopeMove slopeMove = new SlopeMove();
    [SerializeField] private Sliding sliding = new Sliding();
    [SerializeField] private Crouch crouch = new Crouch();
    private StateBase _currentState = new StateBase();
    public PlayerState CurrentStateName => currentStateName;
    
    [Header("Abilities")] 
    [SerializeField] private AirJump airJump;
    [SerializeField] private SlowMotion slowMotion;
    [SerializeField] private CastSphere castSphere;

    private InputController _inputController;

    public static Player This;

    public bool canMove = true;
    
    [field: SerializeField] public Transform GhostRecordPoint { get; private set; }
    
    private void Awake()
    {
        This = this;
        
        mainMove.OnAwake(this.gameObject);
        jump.OnAwake(this.gameObject);
        fall.OnAwake(this.gameObject);
        slopeMove.OnAwake(this.gameObject);
        sliding.OnAwake(this.gameObject);
        crouch.OnAwake(this.gameObject);

        _currentState = mainMove;
        currentStateName = PlayerState.MainMove;

        airJump.OnAwake(this.gameObject);
        slowMotion.OnAwake(this.gameObject);
        castSphere.OnAwake(this.gameObject);
        _inputController = GetComponent<InputController>();
    }
    
    void Start()
    {
        mainMove.OnStart();
        jump.OnStart();
        fall.OnStart();
        slopeMove.OnStart();
        sliding.OnStart();
        crouch.OnStart();
    }

    void Update()
    {
        GroundCheck();
        CeilingCheck();
        SlidingCheck();
        
        if(!canMove) return;
        
        _currentState.OnUpdate();

        airJump.OnUpdate();
        slowMotion.OnUpdate();
        if (_inputController.FirstAbility)
        {
            airJump.OnUse();
        }
        if (_inputController.SecondAbility)
        {
            slowMotion.OnUse();
        }
        if (_inputController.ThirdAbility)
        {
            castSphere.OnUse();
        }
    }

    private void FixedUpdate()
    {
        if(!canMove) return;

        _currentState.OnFixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        Debug.Log(newState);
        switch (newState)
        {
            case (PlayerState.MainMove):
            {
                _currentState.OnExit();
                _currentState = mainMove;
                currentStateName = PlayerState.MainMove;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Jump):
            {
                _currentState.OnExit();
                _currentState = jump; 
                currentStateName = PlayerState.Jump;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Fall):
            {
                _currentState.OnExit();
                _currentState = fall; 
                currentStateName = PlayerState.Fall;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.SlopeMoving):
            {
                _currentState.OnExit();
                _currentState = slopeMove; 
                currentStateName = PlayerState.SlopeMoving;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Sliding):
            {
                _currentState.OnExit();
                _currentState = sliding; 
                currentStateName = PlayerState.Sliding;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Crouch):
            {
                _currentState.OnExit();
                _currentState = crouch; 
                currentStateName = PlayerState.Crouch;
                _currentState.OnEnter();
                break;
            }
            default:
            {
                throw new Exception("invalid state name");
                break;
            }
        }
    }

    public void CastSphereTake()
    {
        ChangeState(PlayerState.Jump);
    }

    #region Checks

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundCheckSphere.offset,
            transform.position.z);
        groundCheckSphere.onGround = Physics.CheckSphere(spherePosition, groundCheckSphere.radius,
            groundCheckSphere.groundsLayers, QueryTriggerInteraction.Ignore);
    }
    private void CeilingCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingCheckSphere.offset,
            transform.position.z);
        ceilingCheckSphere.onCeiling = Physics.CheckSphere(spherePosition, ceilingCheckSphere.radius, ceilingCheckSphere.ceilingLayers,
            QueryTriggerInteraction.Ignore);
    }
    private void SlidingCheck()
    {
        RaycastHit hit;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + slopeCheckSphere.offset,
            transform.position.z);
        
        if (Physics.SphereCast(spherePosition, slopeCheckSphere.radius, Vector3.down, out hit, 
                slopeCheckSphere.length, slopeCheckSphere.slopeLayers, QueryTriggerInteraction.Ignore))
        {
            float slidingAngle = Vector3.Angle(Vector3.up, hit.normal);

            if (slidingAngle >= slopeCheckSphere.minSlopeClamp && slidingAngle <= slopeCheckSphere.maxSlopeClamp) slopeCheckSphere.onSlope = true;
            else slopeCheckSphere.onSlope = false;
        }
        else slopeCheckSphere.onSlope = false;
    }    

    #endregion
    
    #region Gizmos

    private void DrawGroundSphere()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);
        
        if (OnGround) Gizmos.color = green;
        else Gizmos.color = red;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundCheckSphere.offset, transform.position.z);
        
        Gizmos.DrawSphere(spherePosition, groundCheckSphere.radius);
    }
    private void DrawCeilingSphere()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);
        
        if (OnCeiling) Gizmos.color = green;
        else Gizmos.color = red;
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingCheckSphere.offset,
            transform.position.z);
        
        Gizmos.DrawSphere(spherePosition, ceilingCheckSphere.radius);
    }
    private void DrawSlidingSphere()
    {
        Color black = new Color(0f,0f,0f,0.5f);
        Color invis = new Color(0f, 0f, 0f, 0f);
        
        if (OnSlope) Gizmos.color = black;
        else Gizmos.color = invis;
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + slopeCheckSphere.offset - slopeCheckSphere.length, transform.position.z);

        Gizmos.DrawSphere(spherePosition, slopeCheckSphere.radius);
    }
    
    private void OnDrawGizmos()
    {
        if(showGizmos.groundSphere) DrawGroundSphere();
        if(showGizmos.ceilingSphere) DrawCeilingSphere();
        if(showGizmos.slopeSphere) DrawSlidingSphere();
        
        if(showGizmos.stateGizmos) _currentState.OnDrawGizmos();
    }
    
    #endregion
}
