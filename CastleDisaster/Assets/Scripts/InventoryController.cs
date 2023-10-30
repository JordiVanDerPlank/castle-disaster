using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    private void Awake()
    {
        Instance = this;
    }

    
    private void Update()
    {
        resourcesText.text = "Gold: " + resources.ToString(); 
    }

    [SerializeField] int resources;
    [SerializeField] TextMeshProUGUI resourcesText;
    public int GetResources()
    {
        return resources;
    }

    public bool HasEnoughResources(int required)
    {
        return resources >= required;
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }

    public void RemoveResources(int amount)
    {
        resources -= amount;
        if (resources < 0)
        {
            Debug.LogError("This shouldn't be possible!");
            resources = 0;
        }
    }

    public void SetSelectedItem(GameObject prefab, BuildingType buildingType, int cost)
    {
        MouseController.Instance.SetSelectedPrefab(prefab, buildingType, cost);
    }
}
