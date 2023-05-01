using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public bool inputCheck = true;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

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
        if(!inputCheck) return;

        move = value.Get<Vector2>();
    }
    
    public void OnLook(InputValue value)
    {
        if(!inputCheck) return;
        
        look = value.Get<Vector2>();
    }
    
    public void OnJump(InputValue value)
    {
        if(!inputCheck) return;
        
        Jump = value.isPressed;
    }
    
    public void OnSprint(InputValue value)
    {
        if(!inputCheck) return;
        
        _sprint = value.isPressed;
    }
    
    public void OnSlide(InputValue value)
    {
        if(!inputCheck) return;
        
        _slide = value.isPressed;
    }

    public void OnFirstAbility(InputValue value)
    {
        if(!inputCheck) return;
        
        _firstAbility = value.isPressed;
    }
    
    public void OnSecondAbility(InputValue value)
    {
        if(!inputCheck) return;
        
        _secondAbility = value.isPressed;
    }
    
    public void OnThirdAbility(InputValue value)
    {
        if(!inputCheck) return;
        
        _thirdAbility = value.isPressed;
    }
    
    public void OnESC(InputValue value)
    {
        if(!inputCheck) return;
        
        if (Player.This.canMove)
        {
            Player.This.canMove = false;
            UIController.SetWindow((Screen)8);   
        }
        else
        {
            Player.This.canMove = true;
            UIController.SetWindow((Screen)6);
        }
    }
}
