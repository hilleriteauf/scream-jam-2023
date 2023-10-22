using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waving : MonoBehaviour
{

    public float baseLightIntensity = 0.5f;
    public float intensitySpeed = 2f;
    public float intensityAmplitude = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Light>().intensity = baseLightIntensity + (intensityAmplitude * Mathf.Abs(Mathf.Sin(Time.time / intensitySpeed)));
    }
}
