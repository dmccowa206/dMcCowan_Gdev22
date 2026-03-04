using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SimpleHingeInteractable
{
    [SerializeField] Transform doorObject;
    void Start()
    {
        
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
}
