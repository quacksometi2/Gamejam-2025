using TMPro;
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
    public Slider volumeSlider;
    public Toggle muteToggle;
    public TMP_Dropdown graphicsDropdown;
    public CameraFollow camerafollow;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pauseClip;
    public AudioClip resumeClip;
    public AudioClip clickClip;

    private bool isPaused = false;
    private float lastVolume = 1f;

    void Start()
    {
        // Sensitivity
        float sens = PlayerPrefs.GetFloat("Sensitivity", 2f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sens;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        // Volume
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = volume;
        lastVolume = volume;

        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Mute
        if (muteToggle != null)
        {
            muteToggle.isOn = volume == 0f;
            muteToggle.onValueChanged.AddListener(SetMute);
        }

        // Graphics dropdown
        if (graphicsDropdown != null)
        {
            graphicsDropdown.ClearOptions();
            graphicsDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

            int savedLevel = PlayerPrefs.GetInt("GraphicsLevel", QualitySettings.GetQualityLevel());
            graphicsDropdown.value = savedLevel;
            graphicsDropdown.onValueChanged.AddListener(SetGraphicsLevel);

            QualitySettings.SetQualityLevel(savedLevel);
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

        if (audioSource != null && resumeClip != null)
            audioSource.PlayOneShot(resumeClip);
    }

    public void OpenOptions()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);

        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    public void CloseOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);

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

    // Sensitivity
    public void SetSensitivity(float value)
    {
        if (value < 0.1f) value = 0.1f;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();

        // ⚡ Opdater CameraFollow live
        if (camerafollow != null)
        {
            camerafollow.ApplySensitivity(value);
        }
    }

    // Volume
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        lastVolume = value;
        if (muteToggle != null)
            muteToggle.isOn = value == 0f;
    }

    // Mute
    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            lastVolume = AudioListener.volume;
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = lastVolume;
        }

        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;

        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        PlayerPrefs.Save();
    }

    // Graphics
    public void SetGraphicsLevel(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("GraphicsLevel", index);
        PlayerPrefs.Save();
    }
}
