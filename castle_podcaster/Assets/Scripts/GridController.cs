using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridObjectInstance
{
    public GameObject gridGameObject;
    public Vector3 position;

    public GridObjectInstance(GameObject gridGameObject, Vector3 position)
    {
        this.gridGameObject = gridGameObject;
        this.position = position;
    }
}

public class GridController : MonoBehaviour
{
    public static GridController Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] List<GridObjectInstance> gridObjectInstances;

    public bool IsPositionTaken(Vector3 position)
    {
        return gridObjectInstances.Exists(x => x.position == position);
    }

    public void AddToPositionTaken(GameObject gridGameObject, Vector3 position)
    {
        gridObjectInstances.Add(new GridObjectInstance(gridGameObject, position));
    }

    public void DestroyObjectOnPosition(Vector3 position)
    {
        int gridObjectIndex = gridObjectInstances.FindIndex(x => x.position == position);
        Destroy(gridObjectInstances[gridObjectIndex].gridGameObject);
        gridObjectInstances.RemoveAt(gridObjectIndex);
    }
}
