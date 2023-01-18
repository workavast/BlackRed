using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JumpRB : MovementState
{
    [SerializeField] private float jumpImpulse;
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        Debug.Log(PlayerRB.PlayerState.Jump);
        
        base.OnEnter();
        Rigidbody.drag = 0;
        Rigidbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        SpeedController();

        if (Player.OnGround)
        {
            Player.ChangeState(PlayerRB.PlayerState.MainMove);
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
        Vector3 horizontalVelocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        if (horizontalVelocity.magnitude > moveSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * moveSpeed;
            Vector3 verticalVelocity = new Vector3(0, Rigidbody.velocity.y, 0);
            
            Rigidbody.velocity = horizontalVelocity + verticalVelocity;
        }
    }
    
    private void Move()
    {
        Vector3 _moveDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;

        Rigidbody.AddForce(_moveDirection * moveForce, ForceMode.Force);
    }
}
