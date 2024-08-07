using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] private float deltaTimeModif;
    [SerializeField] private float workerDistanceOffset;

    [Header("Stats")]
    [SerializeField] private statStruct worker;
    [SerializeField] private statStruct workerSpeed;
    [SerializeField] private float workerSpeedModif;
    [SerializeField] private statStruct workerStrength;

    [Header("World Refs")]
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform haulLineBegin;
    [SerializeField] private Transform haulLineEnd;

    [HideInInspector] public bool shouldSpawnWorker;

    public static SCR_manager_main instance;
    private void Awake()
    {
        instance = this;

        //entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        //Show intial prices
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerTexts().costText, worker); 
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerSpeedTexts().costText, workerSpeed);
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerStrengthTexts().costText, workerStrength);

        SCR_manager_ui.instance.GetWorkerTexts().currentStatText.text = worker.statCurrent.ToString();
        SCR_manager_ui.instance.GetWorkerSpeedTexts().currentStatText.text = DisplaySpeed();
        SCR_manager_ui.instance.GetWorkerStrengthTexts().currentStatText.text = workerStrength.statCurrent.ToString();
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

    public void Sell(int resource)
    {
        money += resource; //Change Later
        SCR_manager_ui.instance.UpdateMoneyDisplay(money);
        SCR_master_audio.instance.playRandomEffect("MONEY_MADE");
    }

    public float GetWorkerSpeed()
    {
        return workerSpeed.statCurrent * workerSpeedModif;
    }

    public int GetWorkerStrength()
    {
        return workerStrength.statCurrent;
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
        money -= cost;
        stat.n++;
        display.costText.text = "$" + CalculatePolynominalCost(stat).ToString();
        stat.statCurrent++;
        display.currentStatText.text = (stat.statCurrent).ToString();
        SCR_master_audio.instance.playRandomEffect("MONEY_SPENT");
    }
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
}
