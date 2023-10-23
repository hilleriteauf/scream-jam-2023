using System.Collections.Generic;
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
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButton()
    {
        BlackScreenUI blackScreenUI = FindObjectOfType<BlackScreenUI>();
        blackScreenUI.Display(new List<string>(), new List<float>(), false, true, () => { SceneManager.LoadScene(PlaySceneName); return null; }, false);
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
