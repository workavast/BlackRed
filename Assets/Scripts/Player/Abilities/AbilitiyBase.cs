using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiyBase
{
    protected Player Player;
    protected Transform PlayerTransform;

    public virtual void OnAwake(GameObject obj)
    {
        Player = obj.GetComponent<Player>();
        PlayerTransform = obj.transform;
    }

    public virtual void OnStart(){}

    public virtual void OnUse(){}

    public virtual void OnUpdate(){}

    public virtual void OnFixedUpdate(){}
    
    public virtual void OnDrawGizmos(){}
}
