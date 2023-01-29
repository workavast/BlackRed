using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlopeMove : PlayerCameraMoveState
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 1;
    [SerializeField] private float gravity = -1;
    [SerializeField] private float moveStep;

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
    private Vector3 _inertVelocity = Vector3.zero;
    private Vector3 _startInertVelocity = Vector3.zero;
    private Vector3 _slidingVelocity = Vector3.zero;
    private Vector3 _verticalVelocity = Vector3.zero;
    
    public override void OnStart()
    {
        base.OnStart();
    }

    private Vector3 moveTarget = Vector3.zero;
    public override void OnEnter()
    {
        base.OnEnter();
        //_currentSlidingSpeed = 0;

        Vector3 startSpeed = CharacterController.velocity;
        startSpeed.y = 0;
        _currentSpeed = startSpeed.magnitude;
        
        _moveVelocity = startSpeed;
        //_inertVelocity = startSpeed;
        //_startInertVelocity = _inertVelocity;
        
        RaycastHit hit;
        Physics.SphereCast(PlayerTransform.position + Vector3.up, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, groundLayers,
            QueryTriggerInteraction.Ignore);

        moveTarget = hit.normal;
        moveTarget.y = 0;
        // _slidingVelocity = CharacterController.velocity;
        // _currentSlidingSpeed = _slidingVelocity.magnitude;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Slid();
        //Inertia();
        Move();
        Gravity();

        //Debug.Log(_slidingVelocity.magnitude);
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
    
    // void Inertia()
    // {
    //     if (_inertVelocity == Vector3.zero) return;
    //
    //     _inertVelocity -= _inertVelocity * (moveStep * Time.deltaTime);
    //     
    //     if(Mathf.Abs(_startInertVelocity.magnitude - _inertVelocity.magnitude) < moveStep)
    //         _inertVelocity = Vector3.zero;
    // }
    
    private void Gravity()
    {
        if (Player.OnGround && _verticalSpeed < 0)
            _verticalSpeed = -2f;
        
        _verticalSpeed += gravity * Time.deltaTime;
        
        _verticalVelocity = new Vector3(0f,(3f * _verticalSpeed),0f);
    }

    private float Line(float target, float current, float step)
    {
        if (Mathf.Abs(target - current) < (step * Time.deltaTime))
            return target;
        
        float newValue;
        
        if(current > target)
            newValue = current - (step * Time.deltaTime);
        else
            newValue = current + (step * Time.deltaTime);
        
        if (Mathf.Abs(target - newValue) < (step * Time.deltaTime))
            newValue = target;
        
        return newValue;
    }
    
    private void Move()
    {
        if (InputController.move == Vector2.zero)
        {
            float angle = Vector3.Angle(_moveVelocity, moveTarget);
        
            Vector3 newDirection = Vector3.RotateTowards(_moveVelocity, moveTarget, 1.57f * moveStep / 10 * angle/80, 1.57f);
            _moveVelocity = newDirection.normalized * _moveVelocity.magnitude;
            
            if (angle > 90)
                _currentSpeed = Line(0,_currentSpeed, moveAcceleration);
            else
                _currentSpeed = Line(moveSpeed,_currentSpeed, moveAcceleration);
        
            _moveVelocity = _moveVelocity.normalized * _currentSpeed;
            
            return;
        }
        else
        {
            Vector3 inputDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;
            
            RaycastHit hit;
            Physics.SphereCast(PlayerTransform.position + Vector3.up, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, groundLayers,
                QueryTriggerInteraction.Ignore);
            
            float angle = Vector3.Angle(Vector3.up, hit.normal);
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
            }
            
            
            Vector3 newDirection = Vector3.RotateTowards(_moveVelocity, inputDirection, 1.57f * moveStep / 10, 1.57f);
            _moveVelocity = newDirection.normalized * _moveVelocity.magnitude;
        
            float angleV = Vector3.Angle(_moveVelocity, moveTarget);
            
            if (angleV > 90)
                _currentSpeed = Line(0,_currentSpeed, moveAcceleration);
            else
                _currentSpeed = Line(moveSpeed,_currentSpeed, moveAcceleration);
        
            _moveVelocity = _moveVelocity.normalized * _currentSpeed;
        }
    }

}