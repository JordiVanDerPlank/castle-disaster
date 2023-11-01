using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse3D : MonoBehaviour
{
    public LayerMask mouseColliderLayermask;
    public static Mouse3D Instance;

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();
    public static bool IsMouseOverUI() => Instance.IsMouseOverUI_Instance();

    void Awake()
    {
        Instance = this;
    }

    private Vector3 GetMouseWorldPosition_Instance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderLayermask))
            return hit.point;

        return Vector3.zero;
    }

    private bool IsMouseOverUI_Instance()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
