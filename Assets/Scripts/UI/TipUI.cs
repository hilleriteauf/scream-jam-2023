using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipUI : MonoBehaviour
{

    public TextMeshProUGUI tipText;

    private float lastDisplayTime = 0f;
    private float displayDuration = 0f;

    void Awake()
    {
        tipText.gameObject.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - lastDisplayTime) > displayDuration)
        {
            tipText.gameObject.SetActive(false);
        } else if (Time.time > lastDisplayTime)
        {
            tipText.gameObject.SetActive(true);
        }
    }

    public void Display(string text, float duration, float displayLaterTime = 0f)
    {
        tipText.text = text;
        lastDisplayTime = Time.time + displayLaterTime;
        displayDuration = duration;
        tipText.gameObject.SetActive(displayLaterTime == 0f);
    }

}
