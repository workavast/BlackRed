using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

[Serializable]
public class SlowMotion : AbilitiyBase
{
    [SerializeField] protected SomeStorage Reloading;
    [SerializeField] protected float ReloadSpeed;
    [SerializeField] protected float TimeScale;
    private bool used = false;
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
        
        if (used)
        {
            used = false;
            Time.timeScale = 1;
        }
        else
        {
            if (Reloading.MaxValue == Reloading.CurrentValue)
            {
                used = true;
                Time.timeScale = TimeScale;
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (used)
        {
            Reloading.ChangeCurrentValue(-ReloadSpeed * Time.deltaTime);
            if (Reloading.CurrentValue == 0)
            {
                OnUse();
            }
        }
        else
        {
            Reloading.ChangeCurrentValue(ReloadSpeed * Time.deltaTime);
        }
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
