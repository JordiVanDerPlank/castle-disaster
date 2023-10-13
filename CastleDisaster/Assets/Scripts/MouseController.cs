using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] GameObject selectedPrefab;
    int selectedPrefabCost;
    [SerializeField] Transform previewCube;
    Camera _camera;
    [SerializeField] Vector3 coordinates, placementCoordinates;

    private void Awake()
    {
        _camera = Camera.main;
        Instance = this;
    }

    public void SetSelectedPrefab(GameObject prefab, int cost)
    {
        selectedPrefab = prefab;
        selectedPrefabCost = cost;
    }

    GameObject foundBuildSpot;

    // Update is called once per frame
    void Update()
    {
        Vector3 scaled = Vector3.Scale(Input.mousePosition, new Vector3((float)_camera.pixelWidth / Screen.width, (float)_camera.pixelHeight / Screen.height));
        Ray ray = _camera.ScreenPointToRay(scaled);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            //print(hit.collider.gameObject.name);
            coordinates = hit.collider.transform.position;
            return;

            placementCoordinates = new Vector3((int)coordinates.x, (int)coordinates.y + 1, (int)coordinates.z);

            if (IsPointerOverUIObject() || hit.collider.gameObject.tag != "BuildSpot")
            {
                if (foundBuildSpot != null)
                    foundBuildSpot.GetComponent<BuildSpotController>().HidePreview();
                foundBuildSpot = null;
                return;
            }

            print(foundBuildSpot.name);
            foundBuildSpot = hit.collider.gameObject;
            ShowPreview(foundBuildSpot.GetComponent<BuildSpotController>());

            BuildObject();

            DestroyObject();
        }

        else
        {
            HidePreview();
        }
    }

    void ShowPreview(BuildSpotController _foundBuildSpotController)
    {
        //previewCube.gameObject.SetActive(true);
        //while (GridController.Instance.IsPositionTaken(placementCoordinates))
        //    placementCoordinates = new Vector3(placementCoordinates.x, placementCoordinates.y + 1, placementCoordinates.z);
        //previewCube.position = placementCoordinates;

        _foundBuildSpotController.ShowPreview();
    }

    void HidePreview()
    {
        previewCube.gameObject.SetActive(false);
    }

    void BuildObject()
    {
        if (selectedPrefab == null)
            return;

        if (Input.GetMouseButtonDown(0) && !GridController.Instance.IsPositionTaken(placementCoordinates))
        {
            GameObject _newBuilding = Instantiate(selectedPrefab, placementCoordinates, Quaternion.identity);
            GridController.Instance.AddToPositionTaken(_newBuilding, placementCoordinates);
            InventoryController.Instance.RemoveResources(selectedPrefabCost);
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
