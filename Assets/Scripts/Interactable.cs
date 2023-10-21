using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public interface IInteractionListener
    {
        void OnInteract();
    }

    [Tooltip("The script that will be notified when the player interacts with this object. The provided script must implement the IINteractionListener interface.")]
    public MonoBehaviour interactionListenerMono;
    private IInteractionListener interactionListener;

    private bool InPlayerFocus = false;

    void OnValidate()
    {
        if (interactionListenerMono is IInteractionListener)
        {
            interactionListener = (IInteractionListener)interactionListenerMono;
        }
        else
        {
            interactionListenerMono = null;
            interactionListener = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInPlayerFocus(bool inPlayerFocus)
    {
        InPlayerFocus = inPlayerFocus;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null) {
            return;
        }

        if (inPlayerFocus)
        {
            meshRenderer.material.color = Color.blue;
        }
        else
        {
            meshRenderer.material.color = Color.white;
        }
    }
}
