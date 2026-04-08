using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshJoystick : SimpleHingeInteractable
{
    [SerializeField] Transform trackedObject, trackingObject;
    [SerializeField] NavMeshRobot robot;
    [SerializeField] Transform rotationParentObject;
    protected override void ResetHinge()
    {
        if (robot != null)
        {
            robot.StopAgent();
        }
    }
    protected override void Update()
    {
        base.Update();
        if (isSelected)
        {
            MoveRobot();
        }
    }

    private void MoveRobot()
    {
        if (robot != null)
        {
            trackingObject.position = new Vector3(trackedObject.position.x,
                trackingObject.position.y, trackedObject.position.z);
            rotationParentObject.rotation = Quaternion.identity;
            robot.MoveAgent(trackingObject.localPosition);
        }
    }
}
