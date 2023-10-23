using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject mapUI;

    public bool mapObtained = false;

    public bool MapOpened { get; private set; } = false;
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

        if (!MapOpened && Input.GetKey(KeyCode.Tab) && playerInteraction.CanInteract)
        {
            mapUI.SetActive(true);
            MapOpened = true;
            playerInteraction.CanInteract = false;
        }

        if (MapOpened && !Input.GetKey(KeyCode.Tab))
        {
            mapUI.SetActive(false);
            MapOpened = false;
            playerInteraction.CanInteract = true;
        }
    }
}
