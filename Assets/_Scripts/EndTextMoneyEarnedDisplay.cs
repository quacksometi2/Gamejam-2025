using TMPro;
using UnityEngine;

public class EndTextMoneyEarnedDisplay : MonoBehaviour
{
    public TextMeshProUGUI earnedText;

    void Start()
    {
        var moneyManager = MoneyManager.Instance ?? GameObject.FindAnyObjectByType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogWarning("EndTextMoneyEarnedDisplay: MoneyManager not found.");
            return;
        }

        if (earnedText != null)
        {
            earnedText.text = "You earned a total of: "+moneyManager.GetEarnedDuringRun().ToString();
        }
        else
        {
            Debug.LogWarning("EndTextMoneyEarnedDisplay: earnedText reference is missing.");
        }

        Destroy(moneyManager.gameObject);
    }
}
