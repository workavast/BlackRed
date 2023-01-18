using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Jump
{
    // private void GroundedCheck()
    // {
    //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundSphereOffset,
    //         transform.position.z);
    //     onGround = Physics.CheckSphere(spherePosition, groundSphereRadius, groundsLayers,
    //         QueryTriggerInteraction.Ignore);
    // }
    //
    // private void CeilingCheck()
    // {
    //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingSphereOffset,
    //         transform.position.z);
    //     onCeiling = Physics.CheckSphere(spherePosition, ceilingSphereRadius, ceilingLayers,
    //         QueryTriggerInteraction.Ignore);
    // }
    public void OnDrawGizmos()
    {
        Color red = new Color(1f, 0f, 0f, 0.3f);

        Gizmos.color = red;
        Gizmos.DrawSphere(new Vector3(0,0,0), 5);
    }

}
