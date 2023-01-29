using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiyBase
{
    protected Player Player;

    public virtual void OnAwake(GameObject obj)
    {
        Player = obj.GetComponent<Player>();
    }

    public virtual void OnStart(){}

    public virtual void OnUse(){}

    public virtual void OnUpdate(){}

    public virtual void OnFixedUpdate(){}
    
    public virtual void OnDrawGizmos(){}
}
