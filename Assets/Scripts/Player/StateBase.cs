using UnityEngine;

public class StateBase
{
    public virtual void OnAwake(GameObject obj){}

    public virtual void OnStart(){}

    public virtual void OnEnter(){}

    public virtual void OnUpdate(){}

    public virtual void OnFixedUpdate(){}
    
    public virtual void OnExit(){}
    
    public virtual void OnDrawGizmos(){}
}
