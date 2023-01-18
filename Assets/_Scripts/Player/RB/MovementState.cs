using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MovementState : StateBase
{
    [Tooltip("Move speed of the character, m/s")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float moveForce = 10f;

    private GameObject _cinemachineCameraPosition;
    [SerializeField] private float mouseSensitive;
    [SerializeField] private float bottomClamp;
    [SerializeField] private float topClamp;
    private float _cinemachineAngle;
    private float _rotationVelocity;

    public override void OnAwake(GameObject player, GameObject cinemachineCameraPosition)
    {
        base.OnAwake(player, cinemachineCameraPosition);
        _cinemachineCameraPosition = cinemachineCameraPosition;
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
        //Debug.Log(_cinemachineCameraPosition.transform.localRotation.eulerAngles.x);

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
}
