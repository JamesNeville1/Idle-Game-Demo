using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_manager_ui : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyDisplay;
    [SerializeField] private TextMeshProUGUI fpsDisplay;
    [System.Serializable] public struct infoPannelTextStruct
    {
        public TextMeshProUGUI costText;
        public TextMeshProUGUI currentStatText;
    }

    [SerializeField] private infoPannelTextStruct workerText;
    [SerializeField] private infoPannelTextStruct workerStrengthText;
    [SerializeField] private infoPannelTextStruct workerSpeedText;

    public static SCR_manager_ui instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    public void UpdateMoneyDisplay(int raw)
    {
        moneyDisplay.text = "$" + raw.ToString();
    }
    public void UpdateFPSDisplay()
    {
        fpsDisplay.text = "FPS: " + Mathf.Round(1f / Time.unscaledDeltaTime);
    }
    public infoPannelTextStruct GetWorkerTexts() { return workerText; }
    public infoPannelTextStruct GetWorkerStrengthTexts() { return workerStrengthText; }
    public infoPannelTextStruct GetWorkerSpeedTexts() { return workerSpeedText; }
}