using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MainMove : PlayerCameraMoveState
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 1;
    [SerializeField] private float gravity = -1;
    [SerializeField] private float slidingSpeed;
    
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float sphereCastDistance;
    
    private float _currentSpeed = 0;
    private float _verticalSpeed = 0;

    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _verticalVelocity = Vector3.zero;

    public override void OnAwake(GameObject player)
    {
        base.OnAwake(player);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Vector3 startSpeed = CharacterController.velocity;
        startSpeed.y = 0;
        _currentSpeed = startSpeed.magnitude;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Move();
        
        CharacterController.Move((_moveVelocity + _verticalVelocity) * Time.deltaTime);

        if (Player.OnGround && Player.OnSlope)
        {
            Player.ChangeState(PlayerState.SlopeMoving);
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
        
        if (InputController.Slide)
        {
            if(_currentSpeed >= slidingSpeed) Player.ChangeState(PlayerState.Sliding);
            else Player.ChangeState(PlayerState.Crouch);
            
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
        
        Vector3 inputDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;
        
        RaycastHit hit;
        Physics.SphereCast(PlayerTransform.position + Vector3.up, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, groundLayers,
            QueryTriggerInteraction.Ignore);

        float targetSpeed;
        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle != 0)
        {
            Vector3 normal = hit.normal;
            normal.y = 0;
            float moveAngle = Vector3.Angle(normal, inputDirection);
             
            if (moveAngle > 90) moveAngle = 180 - moveAngle;
             
            float velocityScale = Mathf.Abs(moveAngle/90 - 1);
            float velocityDowngrade = (inputDirection.normalized * (moveSpeed *  Mathf.Pow(angle/90,1.75f))).magnitude;
            velocityDowngrade *= velocityScale;

            targetSpeed = ((inputDirection.normalized * moveSpeed) - (inputDirection.normalized * velocityDowngrade)).magnitude;
        }
        else
        {
            targetSpeed = (inputDirection.normalized * moveSpeed).magnitude;
        }
        
        _currentSpeed = Lerp(targetSpeed, _currentSpeed, moveAcceleration, 0.1f, true);
        _moveVelocity = inputDirection.normalized * _currentSpeed;
    }
    
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawSphere(PlayerTransform.position + Vector3.up + Vector3.down * sphereCastDistance, sphereCastRadius);
    }
}
