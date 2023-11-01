using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> grid;
    private int x;
    private int z;

    private PlacedObject objectAtPosition;

    public GridObject(GridSystem<GridObject> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public void AddObject(PlacedObject newObject)
    {
        objectAtPosition = newObject;
        grid.TriggerGridObjectChanged(x, z);
    }

    public void RemoveObject()
    {
        objectAtPosition = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public PlacedObject GetObject()
    {
        return objectAtPosition;
    }

    public bool CanBuild()
    {
        return objectAtPosition == null;
    }

    public override string ToString()
    {
        return x + " " + z + "\n" + objectAtPosition;
    }
}
