using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private int playerMoney = 100;

    public TextMeshProUGUI MoneyText;

    private int EarnedDuringTheCourseOfGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Fail-safe to avoid duplicate persistent managers
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
         MoneyText.text = "Money: "+playerMoney;
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
}
