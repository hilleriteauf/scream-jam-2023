using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackScreenUI : MonoBehaviour
{
    public float backgroundFadeDuration = 0.5f;
    public float beforeAndAfterMessagesPauseDuration = 0.5f;
    public float betweenMessagesPauseDuration = 0.5f;

    public float messageFadeDuration = 0.5f;

    public Image background;
    public TextMeshProUGUI text;

    [Header("Intro")]
    public List<string> introMessages;
    public List<float> introMessageDurations;
    public bool introSkipFadeIn = false;
    public bool introSkipFadeOut = false;
    public string introNextSceneName;
    public bool introAllowSkip = false;

    private float currentDisplayStartTime = 0.0f;
    private float currentDisplayDuration = 0.0f;

    private List<string> messages = new List<string>();
    private List<float> messageDurations = new List<float>();

    private int currentMessageIndex = 0;
    private float currentMessageStartTime = 0.0f;
    private float currentMessageDuration = 0.0f;

    private bool skipFadeOut = false;

    private bool allowSkip = false;

    private Func<Null> onDisplayFinished;

    private static bool displayedSkippableBefore = false;

    // Start is called before the first frame update
    void Start()
    {
        if (introNextSceneName != null && introNextSceneName != "")
        {
            Display(introMessages, introMessageDurations, introSkipFadeIn, introSkipFadeOut, () => { SceneManager.LoadScene(introNextSceneName); return null; }, introAllowSkip);
        } else
        {
            Display(introMessages, introMessageDurations, introSkipFadeIn, introSkipFadeOut, null, introAllowSkip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDisplayDuration == 0.0f)
        {
            return;
        }

        if (messages.Count > 0)
        {
            bool skipMessage = false;

            if (allowSkip && Input.GetKeyUp(KeyCode.Space))
            {
                skipMessage = true;
                float offset = currentMessageDuration + messageFadeDuration * 3 + beforeAndAfterMessagesPauseDuration;

                currentMessageStartTime -= offset;
                currentDisplayStartTime -= offset;
            }

            if (Time.time - currentMessageStartTime > currentMessageDuration + messageFadeDuration * 2)
            {
                currentMessageIndex++;
                if (currentMessageIndex >= messages.Count)
                {
                    // We looped through all the messages, so clear the list
                    messages.Clear();
                    currentMessageIndex = 0;
                    currentMessageStartTime = 0.0f;
                    text.text = "";
                    return;
                }

                if (skipMessage)
                {
                    currentMessageStartTime = Time.time - messageFadeDuration;
                } else
                {
                    currentMessageStartTime = Time.time + beforeAndAfterMessagesPauseDuration;
                }
                currentMessageDuration = messageDurations[currentMessageIndex];
            }
            text.text = messages[currentMessageIndex];

            UpdateMessageAlpha();
        }

        UpdateBackgroundAlpha();
        
        if (Time.time - currentDisplayStartTime > currentDisplayDuration - (skipFadeOut ? backgroundFadeDuration : 0f))
        {
            currentDisplayDuration = 0.0f;
            if (onDisplayFinished != null)
            {
                onDisplayFinished();
            }
        }
    }

    private void UpdateBackgroundAlpha()
    {
        float relativeDisplayTime = Time.time - currentDisplayStartTime;

        float newAlpha = 0f;

        if (relativeDisplayTime <= backgroundFadeDuration)
        {
            newAlpha = relativeDisplayTime / backgroundFadeDuration;
        } else if (relativeDisplayTime > currentDisplayDuration - backgroundFadeDuration)
        {
            Debug.Log("realtive Display time" + relativeDisplayTime);
            newAlpha = 1f - ((relativeDisplayTime - (currentDisplayDuration - backgroundFadeDuration)) / backgroundFadeDuration);
        } else
        {
            newAlpha = 1f;
        }

        background.color = new Color(background.color.r, background.color.g, background.color.b, newAlpha);
    }

    private void UpdateMessageAlpha()
    {
        float relativeDisplayTime = Time.time - currentMessageStartTime;

        //Debug.Log("message relative display time: " + relativeDisplayTime);

        float newAlpha = 0f;

        if (relativeDisplayTime < 0)
        {
               newAlpha = 0f;
        } else if (relativeDisplayTime <= messageFadeDuration)
        {
            newAlpha = relativeDisplayTime / messageFadeDuration;
        } else if (relativeDisplayTime > messageFadeDuration + currentMessageDuration)
        {
            newAlpha = 1f - ((relativeDisplayTime - (messageFadeDuration + currentMessageDuration)) / messageFadeDuration);
        } else
        {
            newAlpha = 1f;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, newAlpha);
    }

    public void Display(List<string> messages, List<float> messageDurations, bool skipFadeIn, bool skipFadeOut, Func<Null> onDisplayFinished, bool allowSkip = false)
    {
        this.messages = messages;
        this.messageDurations = messageDurations;
        this.skipFadeOut = skipFadeOut;
        this.onDisplayFinished = onDisplayFinished;
        this.allowSkip = allowSkip;
        
        float totalMessageDuration = 0.0f;
        foreach (float messageDuration in messageDurations)
        {
            totalMessageDuration += messageDuration;
        }

        currentDisplayDuration = totalMessageDuration + (messageFadeDuration * 2) * messages.Count + backgroundFadeDuration * 2f + beforeAndAfterMessagesPauseDuration * 2 + betweenMessagesPauseDuration * (messages.Count - 1);
        currentDisplayStartTime = Time.time - backgroundFadeDuration * (skipFadeIn ? 1f : 0f);
        currentMessageIndex = 0;

        if (messages.Count > 0)
        {
            currentMessageDuration = messageDurations[currentMessageIndex];
            currentMessageStartTime = Time.time + (skipFadeIn ? 0f : backgroundFadeDuration) + beforeAndAfterMessagesPauseDuration;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            text.gameObject.SetActive(true);
        } else
        {
            text.gameObject.SetActive(false);
        }

        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        background.gameObject.SetActive(true);

        if (allowSkip)
        {
            if (displayedSkippableBefore)
            {
                TipUI tipUI = FindObjectOfType<TipUI>();
                tipUI.Display("Press SPACE to skip", 2f, backgroundFadeDuration + messageFadeDuration);
            }

            displayedSkippableBefore = true;
        }
    }
}
