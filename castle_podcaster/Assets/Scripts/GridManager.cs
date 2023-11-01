using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridManager : MonoBehaviour
{
    public delegate void SelectedObjectChange();
    public event SelectedObjectChange OnSelectedObjectChange;

    public static GridManager Instance;

    [Header("Grid settings")]
    public int gridWidth;
    public int gridHeight;
    public int gridCellSize;
    public Vector3 originPosition;

    [Header("Debug")]
    public bool showDebug;

    private PlacedObjectTypeSO selectedObject;

    private GridSystem<GridObject> gridXZ;
    private PlacedObjectTypeSO.Dir currentDir = PlacedObjectTypeSO.Dir.Down;

    void Awake()
    {
        Instance = this;
        gridXZ = new GridSystem<GridObject>(gridWidth, gridHeight, gridCellSize, originPosition, InitGridElement, showDebug);
    }

    GridObject InitGridElement(GridSystem<GridObject> grid, int x, int z)
    {
        return new GridObject(grid, x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        if (selectedObject == null)
            return Vector3.zero;

        Vector3 mousePos = Mouse3D.GetMouseWorldPosition();
        gridXZ.GetXZ(mousePos, out int x, out int z);

        Vector2Int rotationOffset = selectedObject.GetRotationOffset(currentDir);
        Vector3 objectWorldPosition = gridXZ.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridXZ.GetCellSize();

        return objectWorldPosition;
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (selectedObject == null)
            return Quaternion.Euler(0, 0, 0);
        
        return Quaternion.Euler(0, selectedObject.GetRotationAngle(currentDir), 0);
    }

    public PlacedObjectTypeSO GetSelectedObject()
    {
        return selectedObject;
    }

    public void SetSelectedObject(PlacedObjectTypeSO newObject)
    {
        selectedObject = newObject;

        currentDir = PlacedObjectTypeSO.Dir.Down;
        OnSelectedObjectChange?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedObject != null && !Mouse3D.IsMouseOverUI())
        {
            Vector3 mousePos = Mouse3D.GetMouseWorldPosition();
            gridXZ.GetXZ(mousePos, out int x, out int z);

            List<Vector2Int> objectPositions = selectedObject.GetGridPositionList(new Vector2Int(x, z), currentDir);

            bool canBuild = true;

            foreach (Vector2Int gridPosition in objectPositions)
            {
                if (!gridXZ.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = selectedObject.GetRotationOffset(currentDir);
                Vector3 objectWorldPosition = gridXZ.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridXZ.GetCellSize();

                PlacedObject placedObject = PlacedObject.CreateObject(objectWorldPosition, new Vector2Int(x, z), currentDir, selectedObject);

                foreach (Vector2Int gridPosition in objectPositions)
                {
                    gridXZ.GetGridObject(gridPosition.x, gridPosition.y).AddObject(placedObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = gridXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetObject();

            if (placedObject != null)
            {
                placedObject.DestroySelf();
                List<Vector2Int> objectPositions = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in objectPositions)
                {
                    gridXZ.GetGridObject(gridPosition.x, gridPosition.y).RemoveObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentDir = PlacedObjectTypeSO.GetNextDir(currentDir);
            UtilsClass.CreateWorldTextPopup(currentDir.ToString(), Mouse3D.GetMouseWorldPosition());
        }
    }
}
