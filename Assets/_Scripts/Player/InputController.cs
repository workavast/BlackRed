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
            if (_jump)
            {
                _jump = false;
                return true;
            }
            else
                return false;
        }
        private set { _jump = value; }
    }
    private  bool _jump;
    
    public bool Sprint
    {
        get
        {
            if (_sprint)
            {
                _sprint = false;
                return true;
            }
            else
                return false;
        }
        private set { _sprint = value; }
    }
    private bool _sprint;

    public bool Slide
    {
        get
        {
            if (_slide)
            {
                _slide = false;
                return true;
            }
            else
                return false;
        }
        private set { _slide = value; }
    }
    private bool _slide;
    
    public bool FirstAbility
    {
        get
        {
            if (_firstAbility)
            {
                _firstAbility = false;
                return true;
            }
            else
                return false;
        }
        private set { _firstAbility = value; }
    }
    private bool _firstAbility;
    
    public bool SecondAbility
    {
        get
        {
            if (_secondAbility)
            {
                _secondAbility = false;
                return true;
            }
            else
                return false;
        }
        private set { _secondAbility = value; }
    }
    private bool _secondAbility;
    
    
    public bool ThirdAbility
    {
        get
        {
            if (_thirdAbility)
            {
                _thirdAbility = false;
                return true;
            }
            else
                return false;
        }
        private set { _thirdAbility = value; }
    }
    private bool _thirdAbility;
    
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
        Jump = value.isPressed;
    }
    
    public void OnSprint(InputValue value)
    {
        _sprint = value.isPressed;
    }
    
    public void OnSlide(InputValue value)
    {
        _slide = value.isPressed;
    }

    public void OnFirstAbility(InputValue value)
    {
        _firstAbility = value.isPressed;
    }
    
    public void OnSecondAbility(InputValue value)
    {
        _secondAbility = value.isPressed;
    }
    
    public void OnThirdAbility(InputValue value)
    {
        _thirdAbility = value.isPressed;
    }
}
