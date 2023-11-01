using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectVisual : MonoBehaviour
{
    public float snapTime;
    public float rotationTime;

    public float heightModifier;

    private Transform visual;
    private PlacedObjectTypeSO placedObjectTypeSO;

    // Start is called before the first frame update
    void Start()
    {
        GridManager.Instance.OnSelectedObjectChange += OnGridObjectChanged;
    }

    void OnGridObjectChanged()
    {
        RefreshVisual();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = GridManager.Instance.GetMouseWorldSnappedPosition();

        targetPosition.y += heightModifier;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * snapTime);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, GridManager.Instance.GetPlacedObjectRotation(), Time.deltaTime * rotationTime);
    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        placedObjectTypeSO = GridManager.Instance.GetSelectedObject();

        if (placedObjectTypeSO != null)
        {
            visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;

            this.transform.position = GridManager.Instance.GetMouseWorldSnappedPosition();
            this.transform.rotation = GridManager.Instance.GetPlacedObjectRotation();
        }
    }
}
