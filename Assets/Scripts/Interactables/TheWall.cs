using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[ExecuteAlways]

public class TheWall : MonoBehaviour
{
    public UnityEvent OnDestroy;
    [SerializeField] int columns, rows;
    [SerializeField] GameObject wallCubePrefab;
    [SerializeField] GameObject socketCubePrefab;
    [SerializeField] int socketPosition = 1;
    [SerializeField] XRSocketInteractor wallSocket;
    public XRSocketInteractor GetWallSocket => wallSocket;
    [SerializeField] ExplosiveDevice explosive;
    [SerializeField] List<GeneratedColumn> generatedColumn;
    private GameObject[] wallCubes;
    [SerializeField] float cubeSpacing = 0.05f;
    private Vector3 cubeSize, spawnPosition;
    [SerializeField] bool buildWall, deleteWall, destroyWall;
    [SerializeField] int maxPower;
    [SerializeField] AudioClip destroyWallClip;
    public AudioClip GetDestroyClip => destroyWallClip;
    [SerializeField] AudioClip socketClip;
    public AudioClip GetSocketClip => socketClip;
    void Start()
    {
        if (wallSocket != null)
        {
            wallSocket.selectEntered.AddListener(OnSocketEnter);
            wallSocket.selectEntered.AddListener(OnSocketExit);
        }
        if(explosive != null)
        {
            explosive.OnDetonated.AddListener(OnDestroyWall);
        }
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
        if(generatedColumn.Count >= 1)
        {
            for (int i = 0; i < generatedColumn.Count; i++)
            {
                generatedColumn[i].ActivateColumn();
            }
        }
    }
    private void OnSocketExit(SelectEnterEventArgs arg0)
    {
        if(generatedColumn.Count >= 1)
        {
            for (int i = 0; i < generatedColumn.Count; i++)
            {
                generatedColumn[i].ResetColumn();
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
                GenerateColumn(i, rows, true);
            }
            else
            {
                GenerateColumn(i, rows, false);
            }
            spawnPosition.x += cubeSize.x + cubeSpacing;
        }
    }
    private void GenerateColumn(int index, int height, bool socketed)
    {
        GeneratedColumn tempColumn = new GeneratedColumn();
        tempColumn.InitializeColumn(transform, index, height, socketed);
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
    private void OnDestroyWall()
    {
        if(generatedColumn.Count >= 1)
        {
            for (int i = 0; i < generatedColumn.Count; i++)
            {
                int power = Random.Range(maxPower/2, maxPower);
                generatedColumn[i].DestroyColumn(power);
            }
        }
        OnDestroy?.Invoke();
    }
}
[System.Serializable]
public class GeneratedColumn
{
    [SerializeField] GameObject[] wallCubes;
    [SerializeField] bool isSocketed;
    [SerializeField] int columnIndex;
    private bool isParented;
    private Transform parentObject, columnObject;
    private const string Column_Name = "Column";
    private const string Socketed_Column_Name = "Socketed Column";
    public void InitializeColumn(Transform parent, int index, int rows, bool socketed)
    {
        columnIndex = index;
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
                SetColumnName(cube, columnIndex);
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
    private void SetColumnName(GameObject column, int index)
    {
        if(isSocketed)
        {
            column.name = Socketed_Column_Name;
        }
        else
        {
            column.name = Column_Name;
        }
        column.name += index.ToString();
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
    public void DestroyColumn(int power)
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if(wallCubes[i] != null)
            {
                Rigidbody rb = wallCubes[i].GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                wallCubes[i].transform.SetParent(parentObject);
                power = 2000;
                rb.AddRelativeForce(Random.onUnitSphere * power);
            }
        }
    }
    public void ActivateColumn()
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if(wallCubes[i] != null)
            {
                Rigidbody rb = wallCubes[i].GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
    public void ResetColumn()
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
}