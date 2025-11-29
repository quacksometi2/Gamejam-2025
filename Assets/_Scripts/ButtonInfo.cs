using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    [Header("Colors")]
    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    [Header("Target")]
    public Renderer targetRenderer;

    private bool isChosen;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // Default to red when the scene starts
        ApplyColor(redColor);
        isChosen = false;
    }

    // Call this from other scripts or UI events to mark the button as chosen
    public void ChooseButton()
    {
        if (isChosen) return;

        isChosen = true;
        ApplyColor(greenColor);
    }

    private void ApplyColor(Color color)
    {
        if (targetRenderer != null && targetRenderer.material != null)
            targetRenderer.material.color = color;
    }
}
