using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float distanceThreshold = 3.0f;
    public float angleThreshold = 30.0f;

    public int torchCounter;

    private bool _canInteract = true;
    public bool CanInteract {
        get {
            // We return false if the value was changed in the current frame
            // To avoid catching the key press that changed the value twice
            return _canInteract && canInteractChangedTime != Time.time;
        } set {
            canInteractChangedTime = Time.time;
            _canInteract = value;
        }
    }
    private float canInteractChangedTime = 0f;

    private Interactable currentInteractable = null;

    private InteractKeyUI interactKeyUI;

    // Start is called before the first frame update
    void Start()
    {
        interactKeyUI = FindObjectOfType<InteractKeyUI>();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshCurrentInteractable();

        if (CanInteract && currentInteractable != null && Input.GetKeyUp(KeyCode.E))
        {
            currentInteractable.OnInteract();
        }
    }

    void RefreshCurrentInteractable()
    {
        List<Interactable> interactables = new List<Interactable>(FindObjectsOfType<Interactable>());

        float bestAngle = angleThreshold;
        Interactable bestInteractable = null;

        if (CanInteract)
        {
            foreach (Interactable interactable in interactables)
            {
                Vector3 direction = interactable.transform.position - transform.position;
                float distance = direction.magnitude;
                direction.Normalize();

                float angle = Vector3.Angle(transform.forward, direction);

                if (interactable.IsInteractable && distance < distanceThreshold && angle < angleThreshold && angle < bestAngle)
                {
                    bestAngle = angle;
                    bestInteractable = interactable;
                }
            }
        }

        if (currentInteractable != null && currentInteractable != bestInteractable)
        {
            currentInteractable.SetInPlayerFocus(false);
        }

        if (bestInteractable != null && bestInteractable != currentInteractable)
        {
            bestInteractable.SetInPlayerFocus(true);
        }

        currentInteractable = bestInteractable;

        interactKeyUI.IsVisible = currentInteractable != null;
    }
}
