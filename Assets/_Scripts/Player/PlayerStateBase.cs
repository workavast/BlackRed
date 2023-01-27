using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateBase
{
    protected Player Player;
    protected Transform PlayerTransform;
    protected CharacterController CharacterController;
    protected InputController InputController;
    
    public override void OnAwake(GameObject player)
    {
        Player = player.GetComponent<Player>();
        PlayerTransform = player.GetComponent<Transform>();
        CharacterController = player.GetComponent<CharacterController>();
        InputController = player.GetComponent<InputController>();
    }
    
}
