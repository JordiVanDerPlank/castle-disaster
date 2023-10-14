using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] GameObject selectedPrefab;
    BuildingType selectedPrefabBuildingType;
    int selectedPrefabCost;
    [SerializeField] Transform previewCube;
    Camera _camera;
    [SerializeField] Vector3 coordinates, placementCoordinates;

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
            print(hit.collider.gameObject.tag);
            coordinates = hit.collider.transform.position;
            placementCoordinates = new Vector3((int)coordinates.x, (int)coordinates.y + 7.5f, (int)coordinates.z);

            if (IsPointerOverUIObject() || (hit.collider.gameObject.tag != "BuildingBuildspot" && hit.collider.gameObject.tag != "UnitsBuildspot"))
            {
                return;
            }

            if (selectedPrefabBuildingType == BuildingType.building && hit.collider.gameObject.tag != "BuildingBuildspot" || (selectedPrefabBuildingType == BuildingType.unit && hit.collider.gameObject.tag != "UnitsBuildspot"))
                return;

            ShowPreview();

            BuildObject();

            DestroyObject();
        }
    }

    void ShowPreview()
    {
        print("here");
        previewCube.gameObject.SetActive(true);
        while (GridController.Instance.IsPositionTaken(placementCoordinates))
            placementCoordinates = new Vector3(placementCoordinates.x, placementCoordinates.y + 7.5f, placementCoordinates.z);
        previewCube.position = placementCoordinates;
    }

    void BuildObject()
    {
        if (selectedPrefab == null)
            return;

        if (Input.GetMouseButtonDown(0) && !GridController.Instance.IsPositionTaken(placementCoordinates))
        {
            GameObject _newBuilding = Instantiate(selectedPrefab, new Vector3(placementCoordinates.x, placementCoordinates.y - 2.5f, placementCoordinates.z), Quaternion.identity);
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
