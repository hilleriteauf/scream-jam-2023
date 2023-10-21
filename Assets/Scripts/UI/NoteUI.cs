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
        noteText.text = text;
        noteBackground.SetActive(true);
    }

    public void Hide()
    {
        noteBackground.SetActive(false);
    }
}
