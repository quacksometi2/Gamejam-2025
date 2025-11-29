using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GlassToDisableWhenPlayerHasChoosen;
    private bool PlayerHasChosen = false;

    private List<ButtonInfo> listOfButtons = new List<ButtonInfo>();

    public TextMeshProUGUI TimerText; 

    private float counterTimer = 0;

    private bool levelStarted = false;

    private MoneyManager moneyManager;
    private int moneyWhenWoncache;

    void Start()
    {
        listOfButtons.Clear();
        foreach (ButtonInfo B in GameObject.FindObjectsByType<ButtonInfo>(FindObjectsSortMode.None))
        {
            listOfButtons.Add(B);
        }

        moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();

    }

    public void ChosenSettings(int GambleAmount, float TimeToBeat, int MoneyWhenWon)
    {
        if(!PlayerHasChosen)
        {
            PlayerHasChosen = true;
            counterTimer = TimeToBeat;
            //fjern spillers penge man har gamblet
            moneyManager.ChangePlayerMoney(-GambleAmount);
            moneyWhenWoncache = MoneyWhenWon;
            StartLevel();
        }

    }

    void Update()
    {
        if(levelStarted)
        {
            counterTimer -= Time.deltaTime;
            TimerText.text = "Time left: "+counterTimer.ToString("##.##");
            if(counterTimer <= 0)
            {
                // Well you died
                print("Player Died");
                counterTimer = 0;
                TimerText.text = "Time left: 0";
                levelStarted = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

bool runFinsishedLevelonce = false;
    public void FinishedLevel()
    {
        levelStarted = false;
        if(!runFinsishedLevelonce)
        {
            runFinsishedLevelonce = true;
        //Yay player won
        print("Player won!");
        //reward player
        moneyManager.ChangePlayerMoney(moneyWhenWoncache);
        //Load hub in seconds
        StartCoroutine(LoadLevelInSec());
        }
    }

    IEnumerator LoadLevelInSec()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    void StartLevel()
    {
        GlassToDisableWhenPlayerHasChoosen.SetActive(false);
        TimerText.gameObject.SetActive(true);
        foreach(ButtonInfo b in listOfButtons)
        {
            b.CantChoose();
        }
        levelStarted = true;
    }
 
}
