using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Jump jump = new Jump();
    [SerializeField] private MainMove mainMove = new MainMove();
    [SerializeField] private Sliding sliding = new Sliding();
    
    [Header("Main")]
    [SerializeField] private SomeStorage healthPoints;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputController _inputController;

        [Header("Movement")] 
    [Tooltip("Sprint speed of the character, m/s")]
    [SerializeField] private float sprintSpeed = 10f;
    [Tooltip("Walk speed of the character, m/s")]
    [SerializeField] private float walkSpeed = 6f;
    [Tooltip("Crouch speed of the character, m/s")]
    [SerializeField] private float crouchSpeed = 3f;
    [Tooltip("Acceleration of the movement, m/s")]
    [SerializeField] private float moveAcceleration = 1f;
    
        [Space(10)]
    [Tooltip("Mouse sensitive of the character")]
    [SerializeField] private float mouseSensitive = 3f;
    
        [Space(10)]
    [Tooltip("Gravity force of the character, in fall state")]
    [SerializeField] private float gravity = -10f;
    [Tooltip("Gump height of the character, meters")]
    [SerializeField] private float jumpHeight = 2f;
    [Tooltip("If the character is jump or not")]
    [SerializeField] private bool onJump = true;
    
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
    [SerializeField] private bool onCeiling = true;
    [Tooltip("Offset of the sphere that check ceiling, y axis")]
    [SerializeField] private float ceilingSphereOffset = 2f;
    [Tooltip("Sphere radius that check ceiling")]
    [SerializeField] private float ceilingSphereRadius = 2f;
    [Tooltip("What layers used as ceiling")]
    [SerializeField] private LayerMask ceilingLayers;
    
        [Header("Sliding")] 
    [Tooltip("If the character is sliding or not")] 
    [SerializeField] private bool onSliding;
    [Tooltip("Sliding speed")] 
    [SerializeField] private float slidingSpeed;
    [Tooltip("Length of the sphereCast that check sliding")] 
    [SerializeField] private float sphereCastSlidingLength = 0;
    [Tooltip("Offset of the sphere that check sliding, y axis")]
    [SerializeField] private float sphereCastSlidingOffset = 2f;
    [Tooltip("Radius of the sphereCast that check sliding")] 
    [SerializeField] private float sphereCastSlidingRadius = 0;
    [Tooltip("What layers used as ground")]
    [SerializeField] private LayerMask slidingGroundsLayers;
    [Tooltip("Minimal clamp for sliding")] 
    [SerializeField] private float minSlidingClamp;    
    [Tooltip("Maximal clamp for sliding")] 
    [SerializeField] private float maxSlidingClamp;
    
    
        [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far can you move the camera up, degrees")]
    public float TopClamp = 90.0f;
    [Tooltip("How far can you move the camera down, degrees")]
    public float BottomClamp = -90.0f;

    private float _cinemachineAngle = 0;
    private float _rotationVelocity = 0;
    private float _currentSpeed = 0;
    private float _verticalVelocity = 0;
    
    private void Awake()
    {

    }
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputController = GetComponent<InputController>();
        
        _currentSpeed = sprintSpeed;
    }

    void Update()
    {
        JumpAndGravity();
        Move();
        GroundedCheck();
        CeilingCheck();
        Sliding();
    }

    private void LateUpdate()
    {
        Rotation();
    }
    
    private void JumpAndGravity()
    {
        if (onGround)
        {
            if(_verticalVelocity < 0f) _verticalVelocity = -2f;

            if (_inputController.jump)
            {
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            onJump = false;
        }
        else
        {
            onJump = true;
            _inputController.jump = false;
            
            if (onCeiling && _verticalVelocity > 0)
            {
                _verticalVelocity = 0;
            }
        }

        if (_verticalVelocity < 53f)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Sliding()
    {
        RaycastHit hit;

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + sphereCastSlidingOffset,
            transform.position.z);
        
        if (Physics.SphereCast(spherePosition, sphereCastSlidingRadius, Vector3.down, out hit, 
                0.1f, slidingGroundsLayers, QueryTriggerInteraction.Ignore))
        {
            float slidingAngle = Vector3.Angle(transform.up, hit.normal);
            
            if (slidingAngle >= minSlidingClamp && slidingAngle <= maxSlidingClamp)
            {
                onSliding = true;
                Vector3 slidingDirection = new Vector3(hit.normal.x, 0f, hit.normal.z);
                Quaternion rotateAngle = Quaternion.FromToRotation(Vector3.up, slidingDirection);
                slidingDirection = rotateAngle * hit.normal;

                _characterController.Move(slidingDirection * ((-gravity/10 * slidingAngle/10f) * Time.deltaTime));
            }
            else
            {
                onSliding = false;
            }
        }
        else
        {
            onSliding = false;
        }

    }
    
    private void GroundedCheck()
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
    
    private void Move()
    {
        if(!onJump && !onGround)
            return;
        float targetSpeed = _inputController.sprint ? sprintSpeed : walkSpeed;

        if (_inputController.move == Vector2.zero) targetSpeed = 0f;

        float speedAtTheMoment =
            new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (speedAtTheMoment < targetSpeed - speedOffset && speedAtTheMoment > targetSpeed + speedOffset)
        {
            _currentSpeed = Mathf.Lerp(speedAtTheMoment, targetSpeed, Time.deltaTime * moveAcceleration);
            _currentSpeed = Mathf.Round(_currentSpeed * 1000f) / 1000;
        }
        else
        {
            _currentSpeed = targetSpeed;
        }

        Vector3 inputDirection =
            (_inputController.move.x * transform.right + _inputController.move.y * transform.forward).normalized;
        
        Vector3 verticalMove = new Vector3(0, _verticalVelocity * Time.deltaTime,0);
        _characterController.Move(inputDirection.normalized * (_currentSpeed * Time.deltaTime) + verticalMove);
    }
    
    private void Rotation()
    {
        if (_inputController.look.sqrMagnitude >= 0.01f)
        {
            _cinemachineAngle += _inputController.look.y * mouseSensitive * Time.deltaTime;
            _rotationVelocity = _inputController.look.x * mouseSensitive * Time.deltaTime;

            _cinemachineAngle = AngleClamp(_cinemachineAngle, BottomClamp, TopClamp);
            
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineAngle,0f,0f);
            
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private float AngleClamp(float angle, float min, float max)
    {
        if (angle > 360) angle -= 360;
        if (angle < -360) angle += 360;
        return Mathf.Clamp(angle, min, max);
    }
    
    private void TakeDamage(float damage)
    {
        healthPoints.ChangeCurrentValue(-damage);
    }

    private void OnDrawGizmos()
    {
        Color green = new Color(0f,1f,0f,0.3f);
        Color red = new Color(1f, 0f, 0f, 0.3f);

        if (onGround) Gizmos.color = green;
        else Gizmos.color = red;
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y + groundSphereOffset, transform.position.z),
            groundSphereRadius);

        if (onCeiling) Gizmos.color = green;
        else Gizmos.color = red;
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y + ceilingSphereOffset, transform.position.z),
            ceilingSphereRadius);
        
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y + sphereCastSlidingOffset, transform.position.z),
            sphereCastSlidingRadius);
        
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * sphereCastSlidingLength);

        jump.OnDrawGizmos();
    }
}
