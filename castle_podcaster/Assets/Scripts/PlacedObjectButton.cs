using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObjectButton : MonoBehaviour
{
    public PlacedObjectTypeSO placedObjectTypeSO;
    public Text text;

    public void Initialise()
    {
        this.transform.name = placedObjectTypeSO.name + " _button";
        text.text = placedObjectTypeSO.name;
    }

    public void OnClick()
    {
        GridManager.Instance.SetSelectedObject(placedObjectTypeSO);
    }
}
