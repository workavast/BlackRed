using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MainMoveRb : MovementState
{
    [SerializeField] private float maxWalkAngle;
    [SerializeField] private float raycastDistance;
    RaycastHit _rampHit;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        Debug.Log(PlayerRB.PlayerState.MainMove);

        base.OnEnter();
        Rigidbody.drag = 5;
        Vector3 newVelocity = Rigidbody.velocity;
        newVelocity.y = 0;
        Rigidbody.velocity = newVelocity;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        SpeedController();

        if (!Player.OnGround && !Ramp())
        {
            Player.ChangeState(PlayerRB.PlayerState.Fall);
            return;
        }

        if (InputController.jump)
        {
            Player.ChangeState(PlayerRB.PlayerState.Jump);
            InputController.jump = false;
            return;
        }
    }
    
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Move();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    
    private void SpeedController()
    {
        // Vector3 horizontalVelocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        // if (horizontalVelocity.magnitude > moveSpeed)
        // {
        //     horizontalVelocity = horizontalVelocity.normalized * moveSpeed;
        //     Vector3 verticalVelocity = new Vector3(0, Rigidbody.velocity.y, 0);
        //     
        //     Rigidbody.velocity = horizontalVelocity + verticalVelocity;
        // }
        
        Vector3 velocity = Rigidbody.velocity;
        if (velocity.magnitude > moveSpeed)
        {
            velocity = velocity.normalized * moveSpeed;
            
            Rigidbody.velocity = velocity;
        }
    }

    private float _prevAngle;
    private float _currentAngle;
    
    private void Move()
    {
        
        Vector3 moveDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;
        
        // if (Ramp())
        // {
        //     moveDirection = Vector3.ProjectOnPlane(moveDirection, _rampHit.normal).normalized;
        //     
        //     if (_currentAngle > _prevAngle)
        //     {
        //         Rigidbody.velocity = moveDirection * Rigidbody.velocity.magnitude;
        //     }
        //     else
        //     {
        //         if (_currentAngle < _prevAngle)
        //         {
        //             Rigidbody.velocity = moveDirection * Rigidbody.velocity.magnitude;
        //         }
        //     }
        //
        //     _prevAngle = _currentAngle;
        //     Rigidbody.AddForce(moveDirection * moveForce, ForceMode.Force);
        //
        // }
        // else
        // {
        //     Rigidbody.AddForce(moveDirection * moveForce, ForceMode.Force);
        // }

        if (Ramp())
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, _rampHit.normal).normalized;
            
            if (_currentAngle > _prevAngle)
            {
                SetOnRamp();
                Rigidbody.velocity = moveDirection * Rigidbody.velocity.magnitude;
            }
            else
            {
                if (_currentAngle < _prevAngle)
                {
                    SetOnRamp();
                    Rigidbody.velocity = moveDirection * Rigidbody.velocity.magnitude;
                }
            }
            
            Rigidbody.AddForce(moveDirection * moveForce, ForceMode.Force);
            _prevAngle = _currentAngle;
        }
        else
        {
            Rigidbody.AddForce(moveDirection * moveForce, ForceMode.Force);
        }
    }

    private void SetOnRamp()
    {
        RaycastHit hit;
        Physics.Raycast(PlayerTransform.position + Vector3.up, Vector3.down, out hit, raycastDistance);
        PlayerTransform.position = hit.point;
    }
    
    private bool Ramp()
    {
        if(Physics.Raycast(PlayerTransform.position + Vector3.up, Vector3.down, out _rampHit, raycastDistance))
        {
            float angle = Vector3.Angle(Vector3.up, _rampHit.normal);
            _currentAngle = angle;
            return angle < maxWalkAngle && angle != 0;
        }

        return false;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.black;
        Gizmos.DrawLine(PlayerTransform.position  + Vector3.up, PlayerTransform.position + Vector3.up + Vector3.down * raycastDistance);
    }
}
