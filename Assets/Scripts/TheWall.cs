using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TheWall : MonoBehaviour
{
    [SerializeField] GameObject wallCubePrefab;
    [SerializeField] GameObject socketCubePrefab;
    [SerializeField] XRSocketInteractor wallSocket;
    [SerializeField] GameObject[] wallCubes;
    [SerializeField] float cubeSpacing = 0.05f;
    private Vector3 cubeSize, spawnPosition;
    void Start()
    {
        if (wallCubePrefab != null)
        {
            cubeSize = wallCubePrefab.GetComponent<Renderer>().bounds.size;
        }
        spawnPosition = transform.position;
        BuildWall();
    }
    void Update()
    {
        
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
    private void BuildWall()
    {
        wallCubes = new GameObject[2];
        if (wallCubePrefab != null)
        {
            wallCubes[0] = Instantiate(wallCubePrefab, spawnPosition, transform.rotation, gameObject.transform);
        }
        spawnPosition.y += cubeSize.y + cubeSpacing;
        if(socketCubePrefab != null)
        {
            wallCubes[1] = Instantiate(socketCubePrefab, spawnPosition, transform.rotation, gameObject.transform);
            wallSocket = wallCubes[0].GetComponentInChildren<XRSocketInteractor>();
            if (wallSocket != null)
            {
                wallSocket.selectEntered.AddListener(OnSocketEnter);
                wallSocket.selectEntered.AddListener(OnSocketExit);
            }
        }
        // for (int i = 0; i < wallCubes.Length; i++)
        // {
        //     if (wallCubes[i] != null)
        //     {
        //         wallCubes[i].transform.SetParent(transform);
        //     }
        // }
    }
}