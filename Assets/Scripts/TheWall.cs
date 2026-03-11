
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteAlways]

public class TheWall : MonoBehaviour
{
    [SerializeField] int columns, rows;
    [SerializeField] GameObject wallCubePrefab;
    [SerializeField] GameObject socketCubePrefab;
    [SerializeField] int socketPosition = 1;
    private XRSocketInteractor wallSocket;
    [SerializeField] List<GeneratedColumn> generatedColumn;
    private GameObject[] wallCubes;
    [SerializeField] float cubeSpacing = 0.05f;
    private Vector3 cubeSize, spawnPosition;
    [SerializeField] bool buildWall, deleteWall, destroyWall;
    void Start()
    {
    }
    void Update()
    {
        if(buildWall)
        {
            buildWall = false;
            BuildWall();
        }
        if(deleteWall)
        {
            deleteWall = false;
            for(int i = 0; i < generatedColumn.Count; i++)
            {
                generatedColumn[i].DeleteColumn();
            }
            if (generatedColumn.Count >= 1)
            {
                generatedColumn.Clear();
            }
        }
        if(destroyWall)
        {
            destroyWall = false;
        }
    }
    private void AddSocketWall(GeneratedColumn socketedColumn)
    {
            if (wallCubes[socketPosition] != null)
            {
                Vector3 socketSpawnPos = wallCubes[socketPosition].transform.position;
                DestroyImmediate(wallCubes[socketPosition]);
                wallCubes[socketPosition] = Instantiate(socketCubePrefab, socketSpawnPos, transform.rotation, gameObject.transform);
                socketedColumn.SetCube(wallCubes[socketPosition]);
                wallSocket = wallCubes[socketPosition].GetComponentInChildren<XRSocketInteractor>();
                if (wallSocket != null)
                {
                    wallSocket.selectEntered.AddListener(OnSocketEnter);
                    wallSocket.selectEntered.AddListener(OnSocketExit);
                }
                
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
    private void BuildWall()
    {
        if (wallCubePrefab != null)
        {
            cubeSize = wallCubePrefab.GetComponent<Renderer>().bounds.size;
        }
        spawnPosition = transform.position;
        int socketedColumn = Random.Range(0,columns);
        for (int i = 0; i < columns; i++)
        {
            if (i == socketedColumn)
            {
                GenerateColumn(rows, true);
            }
            else
            {
                GenerateColumn(rows, false);
            }
            spawnPosition.x += cubeSize.x + cubeSpacing;
        }
    }
    private void GenerateColumn(int height, bool socketed)
    {
        GeneratedColumn tempColumn = new GeneratedColumn();
        tempColumn.InitializeColumn(transform, height, socketed);
        wallCubes = new GameObject[height];
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if (wallCubePrefab != null)
            {
                wallCubes[i] = Instantiate(wallCubePrefab, spawnPosition, transform.rotation);
                tempColumn.SetCube(wallCubes[i]);
            }
            spawnPosition.y += cubeSize.y + cubeSpacing;
        }
        if(socketed && socketCubePrefab != null)
        {
            if (socketPosition < 0 || socketPosition >= height)
            {
                socketPosition = 0;
            }
            AddSocketWall(tempColumn);
        }
        generatedColumn.Add(tempColumn);
        spawnPosition.y = transform.position.y;
    }
}
[System.Serializable]
public class GeneratedColumn
{
    [SerializeField] GameObject[] wallCubes;
    [SerializeField] bool isSocketed;
    private bool isParented;
    private Transform parentObject, columnObject;
    private const string Column_Name = "Column";
    public void InitializeColumn(Transform parent, int rows, bool socketed)
    {
        parentObject = parent;
        wallCubes = new GameObject[rows];
        isSocketed = socketed;
    }
    public void SetCube(GameObject cube)
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if (!isParented)
            {
                isParented = true;
                cube.name = Column_Name;
                cube.transform.SetParent(parentObject);
                columnObject = cube.transform;
            }
            else
            {
                cube.transform.SetParent(columnObject);
            }
            if (wallCubes[i] == null)
            {
                wallCubes[i] = cube;
                break;
            }
        }
    }
    public void DeleteColumn()
    {
        for (int i = 0; i< wallCubes.Length; i++)
        {
            if(wallCubes[i] != null)
            {
                Object.DestroyImmediate(wallCubes[i]);
            }
        }
        wallCubes = new GameObject[0];
    }
}