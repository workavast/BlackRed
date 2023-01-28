using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
struct ShowGizmos
{
    [SerializeField] public bool groundSphere;
    [SerializeField] public bool ceilingSphere;
    [SerializeField] public bool slopeSphere;
    [SerializeField] public bool stateGizmos;
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
public class Player : MonoBehaviour
{
    [Header("Gizmos")] 
    [SerializeField] private ShowGizmos showGizmos;
    
    [Header("Ground")] 
    [Tooltip("If the character is grounded or not")]
    [SerializeField] private bool onGround = true;
    [Tooltip("Offset of the sphere that check ground, y axis")]
    [SerializeField] private float groundSphereOffset = 2f;
    [Tooltip("Sphere radius that check ground")]
    [SerializeField] private float groundSphereRadius = 2f;
    [Tooltip("What layers used as ground")]
    [SerializeField] private LayerMask groundsLayers;
    
    [Header("Ceiling")]
    [Tooltip("If the character is ceiling or not")]
    [SerializeField] private bool onCeiling = false;
    [Tooltip("Offset of the sphere that check ceiling, y axis")]
    [SerializeField] private float ceilingSphereOffset = 2f;
    [Tooltip("Sphere radius that check ceiling")]
    [SerializeField] private float ceilingSphereRadius = 2f;
    [Tooltip("What layers used as ceiling")]
    [SerializeField] private LayerMask ceilingLayers;
    
    [Header("Slope")] 
    [Tooltip("If the character is sliding or not")] 
    [SerializeField] private bool onSlope = false;
    [Tooltip("Length of the sphereCast that check sliding")] 
    [SerializeField] private float slopeSphereCastLength = 0;
    [Tooltip("Offset of the sphere that check sliding, y axis")]
    [SerializeField] private float slopeSphereCastOffset = 2f;
    [Tooltip("Radius of the sphereCast that check sliding")] 
    [SerializeField] private float slopeSphereCastRadius = 2f;
    [Tooltip("What layers used as ground")]
    [SerializeField] private LayerMask slopeLayers;
    [Tooltip("Minimal clamp for sliding")] 
    [SerializeField] private float minSlopeClamp;    
    [Tooltip("Maximal clamp for sliding")] 
    [SerializeField] private float maxSlopeClamp;
    
    [Header("States")]
    [SerializeField] private PlayerState currentStateName;
    [SerializeField] private MainMove mainMove = new MainMove();
    [SerializeField] private Jump jump = new Jump();
    [SerializeField] private Fall fall = new Fall();
    [SerializeField] private SlopeMove slopeMove = new SlopeMove();
    [SerializeField] private Sliding sliding = new Sliding();
    [SerializeField] private Crouch crouch = new Crouch();
    private StateBase _currentState = new StateBase();
    
    public bool OnGround => onGround;
    public bool OnCeiling => onCeiling;
    public bool OnSlope => onSlope;
    
    private void Awake()
    {
        mainMove.OnAwake(this.gameObject);
        jump.OnAwake(this.gameObject);
        fall.OnAwake(this.gameObject);
        slopeMove.OnAwake(this.gameObject);
        sliding.OnAwake(this.gameObject);
        crouch.OnAwake(this.gameObject);

        _currentState = mainMove;
        currentStateName = PlayerState.MainMove;
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
        
        _currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
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



    #region Checks

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundSphereOffset,
            transform.position.z);
        onGround = Physics.CheckSphere(spherePosition, groundSphereRadius, groundsLayers,
            QueryTriggerInteraction.Ignore);
    }
    private void CeilingCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingSphereOffset,
            transform.position.z);
        onCeiling = Physics.CheckSphere(spherePosition, ceilingSphereRadius, ceilingLayers,
            QueryTriggerInteraction.Ignore);
    }
    private void SlidingCheck()
    {
        RaycastHit hit;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + slopeSphereCastOffset,
            transform.position.z);
        
        if (Physics.SphereCast(spherePosition, slopeSphereCastRadius, Vector3.down, out hit, slopeSphereCastLength, slopeLayers, QueryTriggerInteraction.Ignore))
        {
            float slidingAngle = Vector3.Angle(Vector3.up, hit.normal);

            if (slidingAngle >= minSlopeClamp && slidingAngle <= maxSlopeClamp) onSlope = true;
            else onSlope = false;
        }
        else onSlope = false;
    }    

    #endregion
    
    #region Gizmos

    private void DrawGroundSphere()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);
        
        if (onGround) Gizmos.color = green;
        else Gizmos.color = red;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundSphereOffset, transform.position.z);
        
        Gizmos.DrawSphere(spherePosition, groundSphereRadius);
    }
    private void DrawCeilingSphere()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);
        
        if (onCeiling) Gizmos.color = green;
        else Gizmos.color = red;
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingSphereOffset,
            transform.position.z);
        
        Gizmos.DrawSphere(spherePosition, ceilingSphereRadius);
    }
    private void DrawSlidingSphere()
    {
        Color black = new Color(0f,0f,0f,0.5f);
        Color invis = new Color(0f, 0f, 0f, 0f);
        
        if (onSlope) Gizmos.color = black;
        else Gizmos.color = invis;
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + slopeSphereCastOffset - slopeSphereCastLength, transform.position.z);

        Gizmos.DrawSphere(spherePosition, slopeSphereCastRadius);
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
