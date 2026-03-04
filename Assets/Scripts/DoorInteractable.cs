using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SimpleHingeInteractable
{
    [SerializeField] CombinationLock comboLock;
    [SerializeField] Transform doorObject;
    void Start()
    {
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
    }
    private void OnUnlocked()
    {
        UnlockHinge();
    }
    private void OnLocked()
    {
        LockHinge();
    }
}
