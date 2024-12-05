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
        public int maxN;
        public int nextCost;
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
        InitialPrice(SCR_manager_ui.instance.GetWorkerTexts().costText, ref worker); 
        InitialPrice(SCR_manager_ui.instance.GetWorkerSpeedTexts().costText, ref workerSpeed);
        InitialPrice(SCR_manager_ui.instance.GetWorkerStrengthTexts().costText, ref workerStrength);

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
            yield return new WaitForSeconds(1f);
        }
    }
    private string DisplaySpeed()
    {
        float speed = workerSpeed.statCurrent * workerSpeedModif + 1;
        return speed.ToString() + "/mph";
    }

    private void InitialPrice(TextMeshProUGUI display, ref statStruct stat)
    {
        int cost = CalculatePolynominalCost(stat);
        stat.nextCost = cost;
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
        if (CheckCost(worker))
        {
            shouldSpawnWorker = true;

            Transaction(SCR_manager_ui.instance.GetWorkerTexts(), ref worker);

            SCR_manager_ui.instance.GetWorkerTexts().currentStatText.text = worker.statCurrent.ToString();
        }
    }
    public void BuyWorkerStrength()
    {
        if (CheckCost(workerStrength))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerStrengthTexts(), ref workerStrength);

            SCR_manager_ui.instance.GetWorkerStrengthTexts().currentStatText.text = workerStrength.statCurrent.ToString();
        }
    }
    public void BuyWorkerSpeed()
    {
        if (CheckCost(workerSpeed))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerSpeedTexts(), ref workerSpeed);

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

    private bool CheckCost(statStruct stat)
    {
        if (stat.maxN > 0)
        {
            if(stat.n + 1 > stat.maxN) 
            {
                return false;
            }
        }

        return (money >= stat.nextCost);
    }
    private int CalculatePolynominalCost(statStruct stat) //Add multiplier pram later
    {
        return stat.initialPrice + stat.increaseRate * (int)Mathf.Pow(stat.n, 2);
    }
    private void Transaction(SCR_manager_ui.infoPannelTextStruct display, ref statStruct stat)
    {

        money -= stat.nextCost; //Pay
        SCR_manager_audio.instance.PlayRandomEffect("MONEY_SPENT"); //SFX

        SCR_manager_ui.instance.UpdateMoneyDisplay(money); //Money Display
        
        //Next Cost Generate
        stat.n++;
        stat.statCurrent++;

        display.currentStatText.text = (stat.statCurrent).ToString();
        if(stat.n >= stat.maxN && stat.maxN > 0)
        {
            display.costText.text = "MAX";
            return;
        }
        else
        {
            int nextCost = CalculatePolynominalCost(stat);

            //Display Text
            display.costText.text = "$" + CalculatePolynominalCost(stat).ToString();

            stat.nextCost = nextCost;
        }
    }
    #endregion
    #endregion
}
