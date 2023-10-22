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
        tipUI.Display("You obtained a map!\nPress Tab to open it", 3f);

        gameObject.SetActive(false);
    }
}
