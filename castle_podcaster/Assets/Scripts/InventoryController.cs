using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    [SerializeField] bool testMode;

    private void Awake()
    {
        Instance = this;

        if (testMode)
            resourcesText.enabled = false;
    }

    
    private void Update()
    {
        if (testMode)
            return;

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
        return testMode ? true : resources >= required;
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }

    public void RemoveResources(int amount)
    {
        if (testMode)
            return;

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
