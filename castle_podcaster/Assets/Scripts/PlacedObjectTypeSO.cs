using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class PlacedObjectTypeSO : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch(dir)
        {
            default:
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
                return Dir.Up;
            case Dir.Up:
                return Dir.Right;
            case Dir.Right:
                return Dir.Down;
        }
    }
    public enum Dir
    {
        Up,
        Down,
        Left,
        Right
    }

    public string objectName;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public bool[] objectMatrix;

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return new Vector2Int(0, 0);
            case Dir.Left:
                return new Vector2Int(0, width);
            case Dir.Up:
                return new Vector2Int(width, height);
            case Dir.Right:
                return new Vector2Int(height, 0);
        }
    }

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return 0;
            case Dir.Left:
                return 90;
            case Dir.Up:
                return 180;
            case Dir.Right:
                return 270;
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> myList = new List<Vector2Int>();
        bool[,] matrix = ConvertMatrixTo2D();

        int rotateMatrixByTimes = 0;
        switch(dir)
        {
            default:
            case Dir.Down:
                rotateMatrixByTimes = 0;
                break;

            case Dir.Left:
                rotateMatrixByTimes = 3;
                break;

            case Dir.Up:
                rotateMatrixByTimes = 2;
                break;

            case Dir.Right:
                rotateMatrixByTimes = 1;
                break;
        }

        for (int i = 0; i < rotateMatrixByTimes; i++)
        {
            matrix = RotateMatrixCounterClockwise(matrix);
        }
        Debug.Log(rotateMatrixByTimes);
        string log = "";
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                log += "" +matrix[x, y];
                if (matrix[x, y] == true)
                {
                    myList.Add(offset + new Vector2Int(x, y));
                }
            }

            Debug.Log(log);
            log = "";
        }

        return myList;
    }

    public void CreateMatrix()
    {
        objectMatrix = new bool[width * height];
        ClearMatrix();
    }

    public void ClearMatrix()
    {
        for (int i = 0; i < objectMatrix.Length; i++)
        {
            objectMatrix[i] = false;
        }
    }

    public bool[,] ConvertMatrixTo2D()
    {
        bool[,] converted = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                converted[x, y] = GetMatrixElement(x, y);
            }
        }    

        return converted;
    }

    public void SetMatrixElement(int x, int y, bool value)
    {
        objectMatrix[x + (y * width)] = value;
    }

    public bool GetMatrixElement(int x, int y)
    {
        return objectMatrix[x + (y * width)];
    }

    static bool[,] RotateMatrixCounterClockwise(bool[,] oldMatrix)
    {
        bool[,] newMatrix = new bool[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }
}
