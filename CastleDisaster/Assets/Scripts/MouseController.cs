using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] GameObject selectedPrefab;
    [SerializeField] Transform previewCube;
    Camera _camera;
    [SerializeField] Vector3 coordinates, placementCoordinates;

    private void Awake()
    {
        _camera = Camera.main;
        Instance = this;
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scaled = Vector3.Scale(Input.mousePosition, new Vector3((float)_camera.pixelWidth / Screen.width, (float)_camera.pixelHeight / Screen.height));

        Ray ray = _camera.ScreenPointToRay(scaled);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            coordinates = hit.collider.transform.position;

            placementCoordinates = new Vector3((int)coordinates.x, (int)coordinates.y + 1, (int)coordinates.z);
            while (GridController.Instance.IsPositionTaken(placementCoordinates))
                placementCoordinates = new Vector3(placementCoordinates.x, placementCoordinates.y + 1, placementCoordinates.z);
            previewCube.position = placementCoordinates;

            if (selectedPrefab == null)
                return;

            if (Input.GetMouseButtonDown(0) && !GridController.Instance.IsPositionTaken(placementCoordinates))
            {
                GameObject _newBuilding = Instantiate(selectedPrefab, placementCoordinates, Quaternion.identity);
                GridController.Instance.AddToPositionTaken(_newBuilding, placementCoordinates);
            }

            if (Input.GetMouseButtonDown(1) && GridController.Instance.IsPositionTaken(coordinates))
            {
                GridController.Instance.DestroyObjectOnPosition(coordinates);
            }
        }
    }
}
