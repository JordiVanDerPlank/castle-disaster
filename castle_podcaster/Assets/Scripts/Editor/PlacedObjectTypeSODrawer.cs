using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlacedObjectTypeSO), false)]
[CanEditMultipleObjects]

public class PlacedObjectTypeSODrawer : Editor
{
    private PlacedObjectTypeSO placedObjectTypeSO => target as PlacedObjectTypeSO;

    private SerializedProperty nameProperty;
    private SerializedProperty prefabProperty;
    private SerializedProperty visualProperty;

    private SerializedProperty matrixProperty;

    private void OnEnable()
    {
        nameProperty = serializedObject.FindProperty("objectName");

        prefabProperty = serializedObject.FindProperty("prefab");
        visualProperty = serializedObject.FindProperty("visual");

        matrixProperty = serializedObject.FindProperty("objectMatrix");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawColumnInputFields();
        EditorGUILayout.Space();

        ClearMatrixButton();
        EditorGUILayout.Space();

        if (placedObjectTypeSO.objectMatrix != null && placedObjectTypeSO.width > 0 && placedObjectTypeSO.height > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(placedObjectTypeSO);
        }
    }

    private void ClearMatrixButton()
    {
        if (GUILayout.Button("Clear matrix"))
        {
            placedObjectTypeSO.CreateMatrix();
        }
    }

    private void DrawColumnInputFields()
    {
        var columnsTemp = placedObjectTypeSO.width;
        var rowsTemp = placedObjectTypeSO.height;

        EditorGUILayout.PropertyField(nameProperty, new GUIContent("Name"));
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(prefabProperty, new GUIContent("Prefab"));
        EditorGUILayout.PropertyField(visualProperty, new GUIContent("Visual"));
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(matrixProperty, new GUIContent("Matrix"));
        EditorGUILayout.Space();

        placedObjectTypeSO.width = EditorGUILayout.IntField("Width", placedObjectTypeSO.width);
        placedObjectTypeSO.height = EditorGUILayout.IntField("Height", placedObjectTypeSO.height);

        if ((placedObjectTypeSO.width != columnsTemp || placedObjectTypeSO.height != rowsTemp) &&
            (placedObjectTypeSO.width >= 0 && placedObjectTypeSO.height >= 0))
        {
            placedObjectTypeSO.CreateMatrix();
        }
    }

    private void DrawBoardTable()
    {
        GUIStyle tableStyle = new GUIStyle("box");

        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        GUIStyle headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 30;
        rowStyle.fixedWidth = 30;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (int row = placedObjectTypeSO.height - 1; row >= 0; row--)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for(int column = 0; column < placedObjectTypeSO.width; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                bool data = EditorGUILayout.Toggle(placedObjectTypeSO.GetMatrixElement(column, row), dataFieldStyle);
                placedObjectTypeSO.SetMatrixElement(column, row, data);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
