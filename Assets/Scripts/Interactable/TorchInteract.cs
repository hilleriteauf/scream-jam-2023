using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TorchInteract : MonoBehaviour, Interactable.IInteractionListener
{

    public GameObject Player;
    public Transform torchPickUpPoint;
    public Camera PlayerCamera;

    PlayerInteraction playerInteraction;


    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        playerInteraction.CanInteract = false;
        Debug.Log("Torch picked up");
        if (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) == 0)
        {
            Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)+1).ToString();
            this.GetComponent<Interactable>().IsInteractable = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            // this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.position = torchPickUpPoint.position;
            this.transform.rotation = torchPickUpPoint.rotation;
            this.transform.parent = PlayerCamera.transform;
            PlayerCamera.GetComponent<MouseLook>().playeraudio = this.GetComponent<AudioSource>();
            PlayerCamera.GetComponent<MouseLook>().torch = this.gameObject;
        } 
        else if (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) >= 2)
        {
            Debug.Log("Torch inventory full !");
            return;
        }
        else
        {
            Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)+1).ToString();
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().AddForce(PlayerCamera.transform.up * 100000000f);
        }
        playerInteraction.CanInteract = true;
    }
}
