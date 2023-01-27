using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public bool Jump
    {
        get
        {
            if (jump)
            {
                jump = false;
                return true;
            }
            else
                return false;
        }

        private set { jump = value; }
    }

    private  bool jump;


    public bool sprint;
    public Vector2 move;
    public Vector3 moveV3;
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
        Jump = value.isPressed;
    }
    
    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }
}
