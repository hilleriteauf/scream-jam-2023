using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{

    public TextMeshProUGUI timerText;

    private PlayerLife playerLife;

    void Awake()
    {
        playerLife = FindObjectOfType<PlayerLife>();        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = FormatTime(playerLife.RemainingTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
