using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, Interactable.IInteractionListener
{
    [Multiline]
    public string noteContent;

    private bool noteOpened = false;

    NoteUI noteUI;
    PlayerInteraction playerInteraction;

    // Start is called before the first frame update
    void Start()
    {
        noteUI = FindObjectOfType<NoteUI>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!noteOpened)
        {
            return;
        }

        if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E)))
        {
            CloseNote();   
        }

        // Calculates distance between player and note
        float distance = Vector3.Distance(transform.position, playerInteraction.transform.position);
        if (distance >= playerInteraction.distanceThreshold * 1.5f)
        {
            CloseNote();
        }
    }

    public void CloseNote()
    {
        if (noteUI != null)
        {
            playerInteraction.CanInteract = true;
            noteOpened = false;
            noteUI.Hide();
        }
    }
    public void OnInteract()
    {
        if (noteUI != null)
        {
            playerInteraction.CanInteract = false;
            noteOpened = true;
            noteUI.Display(noteContent);
        }
    }
}
