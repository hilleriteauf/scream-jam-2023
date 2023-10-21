using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public TextMeshProUGUI noteText;
    public GameObject noteBackground;

    // Start is called before the first frame update
    void Awake()
    {
        noteBackground.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(string text)
    {
        Debug.Log("NoteUI.Display()");
        noteText.text = text;
        noteBackground.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("NoteUI.Hide()");
        noteBackground.SetActive(false);
    }
}
