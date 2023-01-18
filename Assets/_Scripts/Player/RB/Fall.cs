using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fall : MovementState
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        Debug.Log(PlayerRB.PlayerState.Fall);

        base.OnEnter();
        Rigidbody.drag = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Player.OnGround)
        {
            Player.ChangeState(PlayerRB.PlayerState.MainMove);
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
    
    private void Move()
    {
        Vector3 _moveDirection = (InputController.move.x * PlayerTransform.right + InputController.move.y * PlayerTransform.forward).normalized;

        Rigidbody.AddForce(_moveDirection * moveForce, ForceMode.Force);
    }
}
