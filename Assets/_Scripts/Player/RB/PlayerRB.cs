using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerRB : MonoBehaviour
{
    [SerializeField] private SomeStorage healthPoints;

    [SerializeField] private float gravityForce;
    
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
    
    [Header("Sliding")] 
    [Tooltip("If the character is sliding or not")] 
    [SerializeField] private bool onSliding = false;
    [Tooltip("Length of the sphereCast that check sliding")] 
    [SerializeField] private float sphereCastSlidingLength = 0;
    [Tooltip("Offset of the sphere that check sliding, y axis")]
    [SerializeField] private float sphereCastSlidingOffset = 2f;
    [Tooltip("Radius of the sphereCast that check sliding")] 
    [SerializeField] private float sphereCastSlidingRadius = 2f;
    [Tooltip("What layers used as ground")]
    [SerializeField] private LayerMask slidingLayers;
    [Tooltip("Minimal clamp for sliding")] 
    [SerializeField] private float minSlidingClamp;    
    [Tooltip("Maximal clamp for sliding")] 
    [SerializeField] private float maxSlidingClamp;
    
    [Space(10)]
    [SerializeField] private MainMoveRb mainMove;
    [SerializeField] private JumpRB jump;
    [SerializeField] private Fall fall;
    [SerializeField] private SlidingRB sliding;
    [SerializeField] private GameObject cinemachineCameraPosition;

    private PlayerInput _playerInput;
    private InputController _inputController;
    private Rigidbody _rigidbody;
    private StateBase _currentState = new StateBase();

    public bool OnGround => onGround;
    public bool OnCeiling => onCeiling;
    public bool OnSliding => onSliding;
    
    public enum PlayerState
    {
        MainMove,
        Jump,
        Fall,
        Sliding
    }
    private void Awake()
    {
        //GetComponent<Collider>().contactOffset = 0.000000000000000000000000000000000000000000001f;
        _playerInput = GetComponent<PlayerInput>();
        _inputController = GetComponent<InputController>();
        _rigidbody = GetComponent<Rigidbody>();
        
        mainMove.OnAwake(this.gameObject, cinemachineCameraPosition);
        jump.OnAwake(this.gameObject, cinemachineCameraPosition);
        fall.OnAwake(this.gameObject, cinemachineCameraPosition);
        sliding.OnAwake(this.gameObject, cinemachineCameraPosition);
        
        ChangeState(PlayerState.MainMove);
    }

    void Start()
    {
        mainMove.OnStart();
        jump.OnStart();
        fall.OnStart();
        sliding.OnStart();
    }

    void Update()
    {
        GroundCheck();
        CeilingCheck();
        SlidingCheck();

        _currentState.OnUpdate();
    }
    
    void FixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        switch (newState)
        {
            case (PlayerState.MainMove):
            {
                _currentState.OnExit();
                _currentState = mainMove;
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Jump):
            {
                _currentState.OnExit();
                _currentState = jump; 
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Fall):
            {
                _currentState.OnExit();
                _currentState = fall; 
                _currentState.OnEnter();
                break;
            }
            case (PlayerState.Sliding):
            {
                _currentState.OnExit();
                _currentState = sliding; 
                _currentState.OnEnter();
                break;
            }
        }
    }
    
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

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + sphereCastSlidingOffset,
            transform.position.z);
        
        if (Physics.SphereCast(spherePosition, sphereCastSlidingRadius, Vector3.down, out hit, 
                sphereCastSlidingLength, slidingLayers, QueryTriggerInteraction.Ignore))
        {
            float slidingAngle = Vector3.Angle(transform.up, hit.normal);
            
            if (slidingAngle >= minSlidingClamp && slidingAngle <= maxSlidingClamp) onSliding = true;
            else onSliding = false;
        }
        else onSliding = false;
    }

    private void DrawGroundSphere()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);
        
        if (onGround) Gizmos.color = green;
        else Gizmos.color = red;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundSphereOffset,
            transform.position.z);
        
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
        
        if (onSliding) Gizmos.color = black;
        else Gizmos.color = invis;
        
        Vector3 spherePosition = new Vector3(transform.position.x,
            transform.position.y + sphereCastSlidingOffset - sphereCastSlidingLength, transform.position.z);
        
        Gizmos.DrawSphere(spherePosition, sphereCastSlidingRadius);
    }
    private void OnDrawGizmos()
    {
        DrawGroundSphere();
        DrawCeilingSphere();
        DrawSlidingSphere();
        
        _currentState.OnDrawGizmos();
    }
}
