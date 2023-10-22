using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, Interactable.IInteractionListener
{
    public Transform LeverHandle;

    public bool lockOnPull = true;
    public bool isPulled { get; set; } = false;


    public void OnInteract(Interactable interactable)
    {
        if (lockOnPull)
        {
            interactable.IsInteractable = false;
        }
        isPulled = !isPulled;

        LeverHandle.localRotation = Quaternion.Euler(isPulled ? -135f : -45f, 0f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
