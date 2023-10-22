using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, Interactable.IInteractionListener
{

    public void OnInteract(Interactable interactable)
    {
        MapUI mapUI = FindObjectOfType<MapUI>();
        mapUI.mapObtained = true;

        TipUI tipUI = FindObjectOfType<TipUI>();
        Debug.Log("tipUI is null: " + (tipUI == null).ToString());
        tipUI.Display(TipUI.Tip.Map, 3f);

        gameObject.SetActive(false);
    }
}
