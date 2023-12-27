using UnityEngine;
using System;

[Serializable]
public class CastSphere : AbilitiyBase
{
    [SerializeField] protected SomeStorage Reloading;
    [SerializeField] protected float ReloadSpeed;
    [SerializeField] protected GameObject Sphere;

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
        
        UnityEngine.Object.Instantiate(Sphere, PlayerTransform.position, PlayerTransform.rotation);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        
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
