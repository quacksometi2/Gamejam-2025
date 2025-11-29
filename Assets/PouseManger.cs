using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject pauseCanvas;     // Canvas til pause-menu
    public GameObject optionsCanvas;   // Canvas til options-menu

    private bool isPaused = false;

    void Update()
    {
        // Tryk Escape for at toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        optionsCanvas.SetActive(false); // skjul options når vi pauser
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenOptions()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // skift til din menu-scene
    }
}
