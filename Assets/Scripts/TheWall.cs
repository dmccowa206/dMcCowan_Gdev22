using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TheWall : MonoBehaviour
{
    [SerializeField] XRSocketInteractor wallSocket;
    [SerializeField] GameObject[] wallCubes;
    void Start()
    {
        if (wallSocket != null)
        {
            wallSocket.selectEntered.AddListener(OnSocketEnter);
            wallSocket.selectEntered.AddListener(OnSocketExit);
        }
    }
    private void OnSocketEnter(SelectEnterEventArgs arg0)
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if(wallCubes[i] != null)
            {
                Rigidbody rb = wallCubes[i].GetComponent<Rigidbody>();
                rb.isKinematic = false;
            }
        }
    }
    private void OnSocketExit(SelectEnterEventArgs arg0)
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if(wallCubes[i] != null)
            {
                Rigidbody rb = wallCubes[i].GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }
    void Update()
    {
        
    }
}
