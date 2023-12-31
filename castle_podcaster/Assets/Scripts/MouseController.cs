using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] GameObject selectedPrefab;
    BuildingType selectedPrefabBuildingType;
    int selectedPrefabCost;
    [SerializeField] Transform previewCube, previewCubeSpawnpoint;
    Camera _camera;
    [SerializeField] Vector3 coordinates, placementCoordinates;
    [SerializeField] float previewYOffset;

    private void Awake()
    {
        _camera = Camera.main;
        Instance = this;
    }

    public void SetSelectedPrefab(GameObject prefab, BuildingType buildingType, int cost)
    {
        selectedPrefab = prefab;
        selectedPrefabBuildingType = buildingType;
        selectedPrefabCost = cost;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scaled = Vector3.Scale(Input.mousePosition, new Vector3((float)_camera.pixelWidth / Screen.width, (float)_camera.pixelHeight / Screen.height));
        Ray ray = _camera.ScreenPointToRay(scaled);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            coordinates = hit.collider.transform.position;
            placementCoordinates = new Vector3(coordinates.x, coordinates.y + previewYOffset, coordinates.z);

            if (IsPointerOverUIObject() || (hit.collider.gameObject.tag != "BuildingBuildspot" && hit.collider.gameObject.tag != "UnitBuildspot"))
            {
                return;
            }

            if (selectedPrefabBuildingType == BuildingType.building && hit.collider.gameObject.tag != "BuildingBuildspot" || (selectedPrefabBuildingType == BuildingType.unit && hit.collider.gameObject.tag != "UnitBuildspot"))
                return;

            ShowPreview();

            BuildObject();

            DestroyObject();
        }
    }

    void ShowPreview()
    {
        previewCube.gameObject.SetActive(true);
        while (GridController.Instance.IsPositionTaken(placementCoordinates))
            placementCoordinates = new Vector3(placementCoordinates.x, placementCoordinates.y + previewYOffset, placementCoordinates.z);
        previewCube.position = placementCoordinates;
    }

    void BuildObject()
    {
        if (selectedPrefab == null)
            return;

        if (Input.GetMouseButtonDown(0) && !GridController.Instance.IsPositionTaken(placementCoordinates) && InventoryController.Instance.HasEnoughResources(selectedPrefabCost))
        {
            GameObject _newBuilding = Instantiate(selectedPrefab, previewCubeSpawnpoint.position, Quaternion.identity);
            GridController.Instance.AddToPositionTaken(_newBuilding, placementCoordinates);
            InventoryController.Instance.RemoveResources(selectedPrefabCost);
            previewCube.gameObject.SetActive(false);
        }
    }

    void DestroyObject()
    {
        if (Input.GetMouseButtonDown(1) && GridController.Instance.IsPositionTaken(coordinates))
        {
            GridController.Instance.DestroyObjectOnPosition(coordinates);
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;

        return false;
    }
}
