using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlidingRB : MovementState
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnEnter()
    {
        Debug.Log(PlayerRB.PlayerState.Sliding);

        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
