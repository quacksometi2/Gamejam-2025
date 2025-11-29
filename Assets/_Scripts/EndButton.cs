using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour
{
    [Header("Scene")]
    public string endSceneName = "EndScreen";

    [Header("Colors")]
    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    [Header("Target")]
    public Renderer targetRenderer;

    private bool isChosen;
    private bool canChoose = true;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // Default to red when the scene starts
        ApplyColor(redColor);
        isChosen = false;
    }

    void Update()
    {
        if (canChoose && !isChosen && Input.GetKeyDown(KeyCode.E))
        {
            ChooseButton();
        }
    }

    public void CantChoose()
    {
        canChoose = false;
    }

    public void ChooseButton()
    {
        if (!canChoose || isChosen) return;

        isChosen = true;
        ApplyColor(greenColor);
        LoadEndScene();
    }

    private void LoadEndScene()
    {
        if (!string.IsNullOrEmpty(endSceneName))
        {
            SceneManager.LoadScene(endSceneName);
        }
        else
        {
            Debug.LogWarning("EndButton: endSceneName is empty.");
        }
    }

    private void ApplyColor(Color color)
    {
        if (targetRenderer != null && targetRenderer.material != null)
            targetRenderer.material.color = color;
    }
}
