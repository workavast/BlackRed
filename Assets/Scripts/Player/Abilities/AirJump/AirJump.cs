using UnityEngine;
using System;

[Serializable]
public class AirJump : AbilitiyBase
{
    [SerializeField] protected SomeStorage Reloading;
    [SerializeField] protected float ReloadSpeed;
    
    public override void OnAwake(GameObject obj)
    {
        base.OnAwake(obj);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUse()
    {
        base.OnUse();
        if (Reloading.MaxValue == Reloading.CurrentValue && Player.CurrentStateName == PlayerState.Jump)
        {
            Reloading.SetCurrentValue(0);
            Player.ChangeState(PlayerState.Jump);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Reloading.ChangeCurrentValue(ReloadSpeed * Time.deltaTime);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
