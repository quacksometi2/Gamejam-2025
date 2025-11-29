using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PouseManger : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject pauseCanvas;
    public GameObject optionsCanvas;

    [Header("Options UI")]
    public Slider sensitivitySlider;
    public CameraFollow camerafollow;

    [Header("Audio")]
    public AudioSource audioSource;     // Træk AudioSource ind her
    public AudioClip pauseClip;         // Lyd når man pauser
    public AudioClip resumeClip;        // Lyd når man genoptager
    public AudioClip clickClip;         // Lyd ved knaptryk (valgfrit)

    private bool isPaused = false;

    void Start()
    {
        float sens = PlayerPrefs.GetFloat("Sensitivity", 2f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sens;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    void Update()
    {
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
        optionsCanvas.SetActive(false);
        camerafollow.enabled = false;
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 🔊 Afspil pause-lyd
        if (audioSource != null && pauseClip != null)
            audioSource.PlayOneShot(pauseClip);
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        camerafollow.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 🔊 Afspil resume-lyd
        if (audioSource != null && resumeClip != null)
            audioSource.PlayOneShot(resumeClip);
    }

    public void OpenOptions()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);

        // 🔊 Klik-lyd (valgfrit)
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    public void CloseOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);

        // 🔊 Klik-lyd (valgfrit)
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SetSensitivity(float value)
    {
        if (value < 0.1f) value = 0.1f;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }
}
