using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public interface IInteractionListener
    {
        void OnInteract(Interactable interactable);
    }

    [Tooltip("The script that will be notified when the player interacts with this object. The provided script must implement the IINteractionListener interface.")]
    public MonoBehaviour interactionListenerMono;

    public Shader InteractableShader;

    public float OutlineScale = 1.05f;

    public bool IsInteractable = true;

    private IInteractionListener interactionListener;

    private bool InPlayerFocus = false;
    private Material material;

    void OnValidate()
    {
        if (!(interactionListenerMono is IInteractionListener))
        {
            interactionListenerMono = null;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        interactionListener = (IInteractionListener)interactionListenerMono;

        Material[] materials = GetComponent<MeshRenderer>().materials;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].shader.name == InteractableShader.name)
            {
                material = materials[i];
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        if (interactionListener != null)
        {
            interactionListener.OnInteract(this);
        }
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
            material.SetFloat("_Scale", OutlineScale);
        }
        else
        {
            material.SetFloat("_Scale", 0.0f);
        }
    }
}
