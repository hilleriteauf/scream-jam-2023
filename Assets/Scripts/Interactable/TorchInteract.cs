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
        Player = GameObject.Find("Main Camera");
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
        if (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) == 0)
        {   
            Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)+1).ToString();
            GameObject Torch = interactable.gameObject;
            Torch.GetComponent<Interactable>().IsInteractable = false;
            Torch.GetComponent<Rigidbody>().useGravity = false;
            // this.GetComponent<Rigidbody>().isKinematic = true;
            Torch.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Torch.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Torch.transform.position = torchPickUpPoint.position;
            Torch.transform.rotation = torchPickUpPoint.rotation;
            Torch.transform.parent = Player.transform;
            Player.GetComponent<MouseLook>().playeraudio = Torch.GetComponent<AudioSource>();
            Player.GetComponent<MouseLook>().torch = Torch;
        } 
        else if (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) >= 2)
        {
            Debug.Log("Torch inventory full !");
            return;
        }
        else
        {
            Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(Player.GetComponent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)+1).ToString();
            interactable.gameObject.GetComponent<Interactable>().IsInteractable = false;
            Destroy(interactable.gameObject);
        }
    }
}
