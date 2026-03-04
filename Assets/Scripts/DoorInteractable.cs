using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SimpleHingeInteractable
{
    [SerializeField] CombinationLock comboLock;
    [SerializeField] Transform doorObject;
    [SerializeField] Vector3 rotationLimits;
    private Transform startRotation;
    private float startAngleX;
    protected override void Start()
    {
        base.Start();
        startRotation = transform;
        startAngleX = startRotation.localEulerAngles.x;
        if (startAngleX >= 180)
        {
            startAngleX -= 360;
        }
        if (comboLock != null)
        {
            comboLock.UnlockAction += OnUnlocked;
            comboLock.LockAction += OnLocked;
        }        
    }

    protected override void Update()
    {
        base.Update();
        if (doorObject != null)
        {
            doorObject.localEulerAngles = new Vector3(
                doorObject.localEulerAngles.x,
                transform.localEulerAngles.y,
                doorObject.localEulerAngles.z
            );
        }
        if (isSelected)
        {
            CheckLimits();
        }
    }
    private void OnUnlocked()
    {
        UnlockHinge();
    }
    private void OnLocked()
    {
        LockHinge();
    }
    private void CheckLimits()
    {
        float localAngleX = transform.localEulerAngles.x;
        if (localAngleX >= 180)
        {
            localAngleX -= 360;
        }
        if (localAngleX >= startAngleX + rotationLimits.x || localAngleX <= startAngleX - rotationLimits.x)
        {
            ReleaseHinge();
            transform.localEulerAngles = new Vector3(
                startAngleX,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
            );
        }
    }
}
