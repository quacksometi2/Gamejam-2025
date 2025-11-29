using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInfo : MonoBehaviour
{

    [Header("Settings")] 
    public int TimeToBeat = 30;
    public int CostToBegin = 100;
    public int MoneyWhenWon = 150;


    [Header("Manager")]
    public GameManager gameManager;

    [Header("Colors")]
    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    [Header("Type")]
    public bool isEndButton;

    [Header("Target")]
    public Renderer targetRenderer;

    private bool isChosen;
    private bool canChoose = true;

    private MoneyManager moneyManager;

    public TextMeshPro ButtonText;

 public void CantChoose()
    {
        canChoose = false;
    }

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if(gameManager == null)
        {
            gameManager = GameObject.FindAnyObjectByType<GameManager>();
        }

        moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();

        // Default to red when the scene starts
        ApplyColor(redColor);
        isChosen = false;

        ButtonTextUpdate();
    }

    void ButtonTextUpdate()
    {
        ButtonText.text = $"Beat in less than {TimeToBeat} seconds.\nCost: {CostToBegin} chips.\nReturn: {MoneyWhenWon} chips";
    }

    // Call this from other scripts or UI events to mark the button as chosen
    public void ChooseButton()
    {
        if (!canChoose || isChosen) return;
            isChosen = true;
            ApplyColor(greenColor);
            print("End");

            if (isEndButton)
            {
                SceneManager.LoadScene("EndScreen");
                return;
            }
     

    }

    public void DisableSelection()
    {
        canChoose = false;
    }

    private void ApplyColor(Color color)
    {
        if (targetRenderer != null && targetRenderer.material != null)
            targetRenderer.material.color = color;
    }
}
