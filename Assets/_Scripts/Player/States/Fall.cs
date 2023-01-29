using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fall : PlayerCameraMoveState
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 1;
    [SerializeField] private float gravity = -1;
    
    private float _currentSpeed = 0;
    private float _verticalSpeed = 0;

    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _inertVelocity = Vector3.zero;
    private Vector3 _verticalVelocity = Vector3.zero;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Vector3 startSpeed = CharacterController.velocity;
        _verticalVelocity.y = startSpeed.y;
        startSpeed.y = 0;
        _currentSpeed = startSpeed.magnitude;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Move();
        
        CharacterController.Move((_moveVelocity + _verticalVelocity + _inertVelocity) * Time.deltaTime);

        if (Player.OnGround && Player.OnSlope)
        {
            Player.ChangeState(PlayerState.SlopeMoving);
            return;
        }

        if (Player.OnGround)
        {
            Player.ChangeState(PlayerState.MainMove);
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
        
        _moveVelocity = inputDirection.normalized * _currentSpeed;
    }
    
    
    
    
    
    
    
    
}
