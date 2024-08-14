using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_manager_main : MonoBehaviour
{
    [System.Serializable] public struct statStruct
    {
        public int statCurrent;
        public int initialPrice;
        public int increaseRate;
        public int n;
        public bool hasMaxN;
        public int maxN;
    }

    [SerializeField] private int money;
    [SerializeField] [Tooltip("Modifer which speeds up game, used for testing")] private float deltaTimeModif;
    [SerializeField] [Tooltip("How far can a worker be away from buildings")] private float workerDistanceOffset;
    [SerializeField] [Tooltip("(In seconds - E.G. 2 = two seconds)")] private float fpsDisplayUpdatePer;

    [Header("Stats")]
    [SerializeField] private statStruct worker;
    [SerializeField] private statStruct workerSpeed;
    [SerializeField] private float workerSpeedModif;
    [SerializeField] private statStruct workerStrength;

    [HideInInspector] public bool shouldSpawnWorker;

    public static SCR_manager_main instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Show intial prices
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerTexts().costText, worker); 
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerSpeedTexts().costText, workerSpeed);
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerStrengthTexts().costText, workerStrength);

        //Show stats
        SCR_manager_ui.instance.GetWorkerTexts().currentStatText.text = worker.statCurrent.ToString();
        SCR_manager_ui.instance.GetWorkerSpeedTexts().currentStatText.text = DisplaySpeed();
        SCR_manager_ui.instance.GetWorkerStrengthTexts().currentStatText.text = workerStrength.statCurrent.ToString();
        SCR_manager_ui.instance.UpdateMoneyDisplay(money);

        //Load Audio
        SceneManager.LoadScene("SCE_audio", LoadSceneMode.Additive);

        //Start FPS Display
        StartCoroutine(FPSDisplayUpdater());
    }

    #region Display Related
    private IEnumerator FPSDisplayUpdater()
    {
        while (true)
        {
            SCR_manager_ui.instance.UpdateFPSDisplay();
            yield return new WaitForSeconds(fpsDisplayUpdatePer);
        }
    }
    private string DisplaySpeed()
    {
        float speed = workerSpeed.statCurrent * workerSpeedModif + 1;
        return speed.ToString() + "/mph";
    }

    private void DisplayInitialPrice(TextMeshProUGUI display, statStruct stat)
    {
        int cost = CalculatePolynominalCost(stat);
        display.text = "$" + cost.ToString();
    }
    #endregion
    #region "Gets"
    public int GetMoney()
    {
        return money;
    }

    public float GetWorkerDistanceOffset()
    {
        return workerDistanceOffset;
    }

    public float GetDeltaTimeModif()
    {
        return deltaTimeModif;
    }

    public float GetWorkerSpeed()
    {
        return workerSpeed.statCurrent * workerSpeedModif;
    }

    public int GetWorkerStrength()
    {
        return workerStrength.statCurrent;
    }
    #endregion
    #region Main Logic
    #region Buy "_"
    public void BuyWorker()
    {
        int cost = CalculatePolynominalCost(worker);
        if (CheckCost(cost, worker))
        {
            shouldSpawnWorker = true; //I hate this

            Transaction(SCR_manager_ui.instance.GetWorkerTexts(), cost, ref worker);

            SCR_manager_ui.instance.GetWorkerTexts().currentStatText.text = worker.statCurrent.ToString();
        }
    }
    public void BuyWorkerStrength()
    {
        int cost = CalculatePolynominalCost(workerStrength);
        if (CheckCost(cost, workerStrength))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerStrengthTexts(), cost, ref workerStrength);

            SCR_manager_ui.instance.GetWorkerStrengthTexts().currentStatText.text = workerStrength.statCurrent.ToString();
        }
    }
    public void BuyWorkerSpeed()
    {
        int cost = CalculatePolynominalCost(workerSpeed);
        if (CheckCost(cost, workerSpeed))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerSpeedTexts(), cost, ref workerSpeed);

            SCR_manager_ui.instance.GetWorkerSpeedTexts().currentStatText.text = DisplaySpeed();
        }
    }
    #endregion
    #region Secondary Logic
    public void Sell(int resource)
    {
        money += resource; //Change Later
        SCR_manager_ui.instance.UpdateMoneyDisplay(money);
        SCR_manager_audio.instance.PlayRandomEffect("MONEY_MADE");
    }

    private bool CheckCost(int cost, statStruct stat)
    {
        bool atMax = false;
        if (stat.hasMaxN)
        {
            if(stat.n + 1 > stat.maxN) 
            { 
                atMax = true;
            }
        }

        return (money >= cost && !atMax);
    }
    private int CalculatePolynominalCost(statStruct stat) //Add multiplier pram later
    {
        return stat.initialPrice + stat.increaseRate * (int)Mathf.Pow(stat.n, 2);
    }
    private void Transaction(SCR_manager_ui.infoPannelTextStruct display, int cost, ref statStruct stat)
    {
        if(stat.n > stat.maxN && stat.hasMaxN)
        {
            display.costText.text = "MAX";
        }

        money -= cost; //Pay

        SCR_manager_ui.instance.UpdateMoneyDisplay(money); //Money Display
        
        //Next Cost Generate
        stat.n++;
        stat.statCurrent++;

        //Display Text
        display.costText.text = "$" + CalculatePolynominalCost(stat).ToString();
        display.currentStatText.text = (stat.statCurrent).ToString();

        SCR_manager_audio.instance.PlayRandomEffect("MONEY_SPENT"); //SFX
    }
    #endregion
    #endregion
}
