using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Navnet på den scene der skal loades når man trykker Play")]
    public string sceneToLoad = "Level1";  // Sæt navnet i Inspector

    [Header("UI Panels")]
    public GameObject optionsPanel;  // Drag dit options-panel her

    // Kald denne fra din Play-knap
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene-navn mangler!");
        }
    }

    // Kald denne fra din Options-knap
    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    // Kald denne fra din Back-knap i Options
    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    // Kald denne fra din Quit-knap
    public void QuitGame()
    {
        Debug.Log("Spillet lukker...");
        Application.Quit();
    }
}
