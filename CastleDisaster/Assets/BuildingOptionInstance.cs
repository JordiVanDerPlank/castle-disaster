using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOptionInstance : MonoBehaviour
{
    [SerializeField] GameObject gameObjectPrefab;

    public void SelectPrefab()
    {
        InventoryController.Instance.SetSelectedItem(gameObjectPrefab);
    }
}
