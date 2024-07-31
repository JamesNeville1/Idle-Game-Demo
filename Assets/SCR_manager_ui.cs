using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_manager_ui : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyDisplay;

    [SerializeField] private TextMeshProUGUI workerCostText;
    [SerializeField] private TextMeshProUGUI workerStrengthCostText;
    [SerializeField] private TextMeshProUGUI workerSpeedCostText;

    public static SCR_manager_ui instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    public void UpdateMoneyDisplay(int raw)
    {
        moneyDisplay.text = "$" + raw.ToString();
    }

    public TextMeshProUGUI GetWorkerCostText() { return workerCostText; }
    public TextMeshProUGUI GetWorkerStrengthCostText() { return workerStrengthCostText; }
    public TextMeshProUGUI GetWorkerSpeedCostText() { return workerSpeedCostText; }
}