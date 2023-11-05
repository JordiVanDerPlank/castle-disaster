using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpotController : MonoBehaviour
{
    [SerializeField] GameObject previewObject, objectToSpawn;
    [SerializeField] Collider collider;
    [SerializeField] GameObject costCanvas;

    private void OnMouseOver()
    {
        ShowPreview();

        if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(objectToSpawn, new Vector3(transform.position.x - 2.25f, transform.position.y, transform.position.z - 2.25f), Quaternion.identity, transform);
            objectToSpawn.SetActive(true);
            collider.enabled = false;
        }
    }

    private void OnMouseExit()
    {
        print("gone");
        HidePreview();
    }

    public void ShowPreview()
    {
        previewObject.SetActive(true);
        costCanvas.SetActive(true);
    }

    public void HidePreview()
    {
        previewObject.SetActive(false);
        costCanvas.SetActive(false);
    }
}
