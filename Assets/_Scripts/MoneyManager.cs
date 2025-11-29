using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private int playerMoney = 100;

    public TextMeshProUGUI MoneyText;

    private int EarnedDuringTheCourseOfGame;

    private void Awake()
    {
        // Keep the very first MoneyManager alive; destroy later duplicates that sneak in.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Start()
    {
         MoneyText.text = "Money: "+playerMoney;
         CheckForZeroMoneyAndEnd();
    }

    public bool CanISpendThis(int money)
    {
        if(playerMoney < money)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ChangePlayerMoney(int change)
    {
        playerMoney += change;
        MoneyText.text = "Money: "+playerMoney;
        if(change > 0)
        {
        EarnedDuringTheCourseOfGame += change;
        }

    }

    public int GetEarnedDuringRun()
    {
        return EarnedDuringTheCourseOfGame;
    }

    public int GetPlayerMoney()
    {
        print("Player current money: "+playerMoney);
        return playerMoney;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckForZeroMoneyAndEnd();
    }

    private void CheckForZeroMoneyAndEnd()
    {
        if (playerMoney <= 0 && SceneManager.GetActiveScene().name != "EndScreen")
        {
            SceneManager.LoadScene("EndScreen");
        }
    }
}
