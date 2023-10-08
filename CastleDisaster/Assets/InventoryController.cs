using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedItem(GameObject prefab)
    {
        MouseController.Instance.SetSelectedPrefab(prefab);
    }
}
