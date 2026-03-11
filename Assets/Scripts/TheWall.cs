using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TheWall : MonoBehaviour
{
    [SerializeField] int columns, rows;
    [SerializeField] GameObject wallCubePrefab;
    [SerializeField] GameObject socketCubePrefab;
    [SerializeField] int socketPosition = 1;
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
        for (int i = 0; i < columns; i++)
        {
            GenerateColumn(rows, true);
            spawnPosition.y = transform.position.y;
            spawnPosition.x += cubeSize.x + cubeSpacing;
        }
    }
    private void GenerateColumn(int height, bool socketed)
    {
        wallCubes = new GameObject[height];
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if (wallCubePrefab != null)
            {
                wallCubes[i] = Instantiate(wallCubePrefab, spawnPosition, transform.rotation, gameObject.transform);
            }
            if (wallCubes[i] != null)
            {
                if (i != 0 && wallCubes[0] != null)
                {
                    wallCubes[i].transform.SetParent(wallCubes[0].transform);
                }
                else if (i == 0)
                {
                    wallCubes[i].name = "Column";
                }
            }
            spawnPosition.y += cubeSize.y + cubeSpacing;
        }
        if(socketed && socketCubePrefab != null)
        {
            if (socketPosition < 0 || socketPosition >= height)
            {
                socketPosition = 0;
            }
            if (wallCubes[socketPosition] != null)
            {
                Vector3 socketSpawnPos = wallCubes[socketPosition].transform.position;
                DestroyImmediate(wallCubes[socketPosition]);
                wallCubes[socketPosition] = Instantiate(socketCubePrefab, socketSpawnPos, transform.rotation, gameObject.transform);
                if(socketPosition == 0)
                {
                    // for(int i = 0; i < wallCubes.Length; i++)
                    // {
                    //     if(i != socketPosition)
                    //     {
                    //         wallCubes[i].transform.SetParent(wallCubes[socketPosition].transform);
                    //     }
                    // }
                }
                else
                {
                    wallCubes[socketPosition].transform.SetParent(wallCubes[0].transform);
                }
                wallSocket = wallCubes[socketPosition].GetComponentInChildren<XRSocketInteractor>();
                if (wallSocket != null)
                {
                    wallSocket.selectEntered.AddListener(OnSocketEnter);
                    wallSocket.selectEntered.AddListener(OnSocketExit);
                }
                
            }
        }
    }
}