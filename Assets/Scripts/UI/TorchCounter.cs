using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TorchCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    private PlayerInteraction playerInteraction;

    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerInteraction.torchCounter.ToString();
    }
}
