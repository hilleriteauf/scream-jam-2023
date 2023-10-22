using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public RectTransform MenuPanel;
    public RectTransform CreditsPanel;

    public string PlaySceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(PlaySceneName);
    }

    public void OnCreditsButton()
    {
        MenuPanel.gameObject.SetActive(false);
        CreditsPanel.gameObject.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnBackButton()
    {
        MenuPanel.gameObject.SetActive(true);
        CreditsPanel.gameObject.SetActive(false);
    }
}
