using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public bool jump;
    public bool sprint;
    public Vector2 move;
    public Vector2 look;
    
    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }
    
    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    
    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }
    
    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }
}
