using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuAndOptions : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "Level1";

    [Header("UI Canvases")]
    public GameObject mainMenuCanvas;   // Canvas - menu
    public GameObject optionsCanvas;    // Canvas - Options

    [Header("Options UI")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        // Load saved volume
        float vol = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = vol;
        if (volumeSlider != null)
        {
            volumeSlider.value = vol;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Load saved sensitivity
        float sens = PlayerPrefs.GetFloat("Sensitivity", 2f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sens;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        // Load saved quality level
        int quality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(quality, true);

        // Show main menu, hide options
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    // Quality settings via buttons
    public void SetQualityLow() { SetQualityByIndex(0); }
    public void SetQualityMedium() { SetQualityByIndex(2); }
    public void SetQualityHigh() { SetQualityByIndex(4); }
    public void SetQualityUltra() { SetQualityByIndex(QualitySettings.names.Length - 1); }

    void SetQualityByIndex(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();
    }
}
