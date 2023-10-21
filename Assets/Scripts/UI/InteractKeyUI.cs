using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKeyUI : MonoBehaviour
{
    public GameObject interactKey;

    public void Awake()
    {
        interactKey.SetActive(false);
    }

    public bool IsVisible
    {
        get
        {
            return interactKey.activeSelf;
        }
        set
        {
            interactKey.SetActive(value);
        }
    }
}
