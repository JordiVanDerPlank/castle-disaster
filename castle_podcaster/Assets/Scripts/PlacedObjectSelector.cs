using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectSelector : MonoBehaviour
{
    public List<PlacedObjectTypeSO> placedObjects;

    public Transform buttonPrefab;
    public Transform buttonParent;
    public float buttonWidth;
    public float margin;

    private void Start()
    {
        foreach (PlacedObjectTypeSO placedObject in placedObjects)
        {
            Transform buttonTransform = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
            buttonTransform.SetParent(buttonParent);

            PlacedObjectButton placedObjectButton = buttonTransform.GetComponent<PlacedObjectButton>();

            if (placedObjectButton != null)
            {
                placedObjectButton.placedObjectTypeSO = placedObject;
                placedObjectButton.Initialise();
            }
            else
            {
                Debug.LogWarning(buttonTransform.name + " has no PlacedObjectButton script attached");
            }
        }

        RectTransform contentRect = buttonParent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(buttonWidth * placedObjects.Count + margin, contentRect.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform contentRect = buttonParent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(buttonWidth * placedObjects.Count + margin, contentRect.sizeDelta.y);

        if (Input.GetKey(KeyCode.Escape))
        {
            GridManager.Instance.SetSelectedObject(null);
        }
    }
}
