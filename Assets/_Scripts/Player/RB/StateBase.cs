using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase
{
    protected PlayerRB Player;
    protected Transform PlayerTransform;
    protected Rigidbody Rigidbody;
    protected InputController InputController;
    
    public virtual void OnAwake(GameObject player, GameObject cinemachineCameraPosition)
    {
        Player = player.GetComponent<PlayerRB>();
        PlayerTransform = player.GetComponent<Transform>();
        Rigidbody = player.GetComponent<Rigidbody>();
        InputController = player.GetComponent<InputController>();
    }

    public virtual void OnStart(){}

    public virtual void OnEnter(){}

    public virtual void OnUpdate(){}

    public virtual void OnFixedUpdate(){}
    
    public virtual void OnExit(){}
    
    public virtual void OnDrawGizmos(){}
}
