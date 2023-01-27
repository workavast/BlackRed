using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlopeMoving : MovementState
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 1;
    [SerializeField] private float gravity = -1;

    [SerializeField] private float inertRotateAcceleration;
    
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slidingAcceleration;
    [SerializeField] private float inertiaAcceleration;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float sphereCastDistance;
    
    private float _currentSlidingSpeed = 0;
    private float _currentSpeed = 0;
    private float _verticalSpeed = 0;

    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _slidingVelocity = Vector3.zero;
    private Vector3 _inertVelocity = Vector3.zero;
    private Vector3 _verticalVelocity = Vector3.zero;
    
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _currentSlidingSpeed = 0;

        Vector3 startSpeed = CharacterController.velocity;
        startSpeed.y = 0;
        _currentSpeed = startSpeed.magnitude;
        
        _inertVelocity = CharacterController.velocity;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Slid();
        Inertia();
        Move();
        
        CharacterController.Move((_moveVelocity + _verticalVelocity + _inertVelocity + _slidingVelocity) * Time.deltaTime);
        
        if (!Player.OnSlope && Player.OnGround)
        {
            Player.ChangeState(PlayerState.MainMove);
            return;
        }

        if (!Player.OnGround)
        {
            Player.ChangeState(PlayerState.Fall);
            return;
        }
        
        if (InputController.Jump)
        {
            Player.ChangeState(PlayerState.Jump);
            return;
        }
    }
    
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }


    private void Slid()
    {
        if (Player.OnSlope)
        {
            RaycastHit hit;
            Physics.SphereCast(PlayerTransform.position + Vector3.up, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, groundLayers,
                QueryTriggerInteraction.Ignore);
            
            float slidingAngle = Vector3.Angle(PlayerTransform.up, hit.normal);
            float targetSpeed = slidingSpeed * slidingAngle/50f;

            _currentSlidingSpeed = Lerp(targetSpeed, _currentSlidingSpeed, slidingAcceleration, 0.01f, false);

            Vector3 slidingDirection = hit.normal;
            slidingDirection.y = 0;
            slidingDirection = Vector3.ProjectOnPlane(slidingDirection, hit.normal).normalized;
            
            _slidingVelocity = slidingDirection * _currentSlidingSpeed;
        }
    }
    
    void Inertia()
    {
        if (_inertVelocity == Vector3.zero) return;
        
        float newInertia = Lerp(0, _inertVelocity.magnitude, inertiaAcceleration, 0.1f, false);

        if (InputController.move != Vector2.zero)
        {
            Vector3 inputDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;
            float newX = Lerp(inputDirection.x, _inertVelocity.x, inertRotateAcceleration, 0.01f, false);
            float newZ = Lerp(inputDirection.z, _inertVelocity.z, inertRotateAcceleration, 0.01f, false);
            
            _inertVelocity.x = newX;
            _inertVelocity.z = newZ;
        }
        
        _inertVelocity = _inertVelocity.normalized * newInertia;
    }
    
    private void Gravity()
    {
        if (Player.OnGround && _verticalSpeed < 0)
            _verticalSpeed = -2f;
        
        _verticalSpeed += gravity * Time.deltaTime;
        
        _verticalVelocity = new Vector3(0f,(3f * _verticalSpeed),0f);
    }
    private void Move()
    {
        Gravity();
        
        _currentSpeed = Lerp(moveSpeed,_currentSpeed,moveAcceleration, 0.1f, true);

        Vector3 inputDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;
        
        RaycastHit hit;
        Physics.SphereCast(PlayerTransform.position + Vector3.up, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, groundLayers,
            QueryTriggerInteraction.Ignore);

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle != 0)
        {
            Vector3 normal = hit.normal;
            normal.y = 0;
            float moveAngle = Vector3.Angle(normal, inputDirection);

            if (moveAngle > 90)
            {
                Vector3 xzNormal = hit.normal;
                xzNormal.y = 0;
                Vector3 paral = Vector3.RotateTowards(xzNormal, inputDirection, 1.57f, 0f);
                inputDirection = Vector3.Project(inputDirection, paral);
                inputDirection /= 2;
                moveAngle = 180 - moveAngle;
            }
            
            float velocityScale = Mathf.Abs(moveAngle/90 - 1);
            float velocityDowngrade = (inputDirection * (_currentSpeed *  Mathf.Pow(angle/90,1.75f))).magnitude;
            velocityDowngrade *= velocityScale;
            
            _moveVelocity = (inputDirection * _currentSpeed) - (inputDirection * velocityDowngrade);
        }
        else
        {
            _moveVelocity = inputDirection * _currentSpeed;
        }
    }

}