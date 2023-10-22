using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject mapUI;

    public bool mapObtained = false;

    private bool mapOpened = false;
    private PlayerInteraction playerInteraction;

    // Start is called before the first frame update
    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapObtained)
        {
            return;
        }

        if (!mapOpened && Input.GetKey(KeyCode.Tab) && playerInteraction.CanInteract)
        {
            mapUI.SetActive(true);
            mapOpened = true;
            playerInteraction.CanInteract = false;
        }

        if (mapOpened && !Input.GetKey(KeyCode.Tab))
        {
            mapUI.SetActive(false);
            mapOpened = false;
            playerInteraction.CanInteract = true;
        }
    }
}
