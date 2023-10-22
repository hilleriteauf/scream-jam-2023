using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private PlayerLife playerLife;

    void Awake()
    {
        playerLife = FindObjectOfType<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerLife.Health;
    }
}
