using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TorchInteract : MonoBehaviour, Interactable.IInteractionListener
{

    public GameObject Player;
    public Transform torchPickUpPoint;

    PlayerInteraction playerInteraction;


    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        torchPickUpPoint = GameObject.Find("TorchPickUpPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(Interactable interactable)
    {
        Debug.Log("Torch picked up");
        if (playerInteraction.torchCounter == 0)
        {   
            playerInteraction.torchCounter += 1;
            Destroy(gameObject);
        } 
        else if (playerInteraction.torchCounter >= 2)
        {
            TipUI tipUI = FindObjectOfType<TipUI>();
            tipUI.Display("You can only carry 2 torches at a time", 3f);
            return;
        }
        else
        {
            playerInteraction.torchCounter += 1;
            interactable.gameObject.GetComponent<Interactable>().IsInteractable = false;
            Destroy(interactable.gameObject);
        }
    }
}
