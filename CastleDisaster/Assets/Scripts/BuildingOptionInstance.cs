using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingOptionInstance : MonoBehaviour
{
    [SerializeField] GameObject gameObjectPrefab;
    [SerializeField] BuildingType buildingType;
    [SerializeField] int cost;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.interactable = InventoryController.Instance.HasEnoughResources(cost);
    }

    public void SelectPrefab()
    {
        InventoryController.Instance.SetSelectedItem(gameObjectPrefab, buildingType, cost);
    }
}
