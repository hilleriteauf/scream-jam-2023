using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, Interactable.IInteractionListener
{
    [Multiline]
    public string noteContent;

    private bool noteOpened = false;

    private float lastOpenTime = 0f;

    NoteUI noteUI;
    PlayerInteraction playerInteraction;

    void Awake()
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

        // We make sure the note has not been opened in the current frame
        // Otherwise we might catch the key press that opened the note
        if (Time.time != lastOpenTime && (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E)))
        {
            Debug.Log("Closing note because of key press");
            CloseNote();   
        }

        // Calculates distance between player and note
        float distance = Vector3.Distance(transform.position, playerInteraction.transform.position);
        if (distance >= playerInteraction.distanceThreshold * 1.5f)
        {
            Debug.Log("Closing note because of distance");
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
        Debug.Log("noteUI is null: " + (noteUI == null).ToString());
        if (noteUI != null)
        {
            Debug.Log("Note.OnInteract()");
            lastOpenTime = Time.time;
            playerInteraction.CanInteract = false;
            noteOpened = true;
            noteUI.Display(noteContent);
        }
    }
}
