using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class MovementState : PlayerStateBase
{
    [Tooltip("Move speed of the character, m/s")]
    [SerializeField] private float mouseSensitive = 1f;
    [SerializeField] private float topClamp = 89f;
    [SerializeField] private float bottomClamp = -89f;
    
    private GameObject _cinemachineCameraPosition;
    private float _cinemachineAngle;
    private float _rotationVelocity;

    public override void OnAwake(GameObject player)
    {
        base.OnAwake(player);
        _cinemachineCameraPosition = PlayerTransform.Find("CameraPosition").gameObject;
    }

    public override void OnEnter()
    {
        _cinemachineAngle = _cinemachineCameraPosition.transform.localRotation.eulerAngles.x;
        
        if (_cinemachineAngle > 90f)
            _cinemachineAngle = (_cinemachineAngle - 270) - 90;
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        Rotation();
    }
    
    private void Rotation()
    {
        if (InputController.look.sqrMagnitude >= 0.01f)
        {
            _cinemachineAngle += InputController.look.y * mouseSensitive * Time.deltaTime;
            _rotationVelocity = InputController.look.x * mouseSensitive * Time.deltaTime;

            _cinemachineAngle = AngleClamp(_cinemachineAngle, bottomClamp, topClamp);
            
            _cinemachineCameraPosition.transform.localRotation = Quaternion.Euler(_cinemachineAngle,0f,0f);
            
            PlayerTransform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    
    private float AngleClamp(float angle, float min, float max)
    {
        if (angle > 360) angle -= 360;
        if (angle < -360) angle += 360;
        return Mathf.Clamp(angle, min, max);
    }

    protected float Lerp(float targetValue, float currentValue, float acceleration, float offset, bool checkInput)
    {
        if (checkInput && InputController.move == Vector2.zero) targetValue = 0f;
        
        if (currentValue < targetValue - offset || currentValue > targetValue + offset)
            currentValue = Mathf.Lerp(currentValue, targetValue, acceleration * Time.deltaTime);
        else
            currentValue = targetValue;

        return currentValue;
    }
}
