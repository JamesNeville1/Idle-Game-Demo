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
    }

    [SerializeField] private int money;
    [SerializeField] private float deltaTimeModif;

    [Header("Stats")]
    [SerializeField] private statStruct worker;
    [SerializeField] private statStruct workerSpeed;
    [SerializeField] private statStruct workerStrength;

    [Header("World Refs")]
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform haulLineBegin;
    [SerializeField] private Transform haulLineEnd;

    public bool shouldSpawnWorker;

    public static SCR_manager_main instance;
    private void Awake()
    {
        instance = this;

        //entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        //Show intial prices
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerCostText(), worker); 
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerSpeedCostText(), workerSpeed);
        DisplayInitialPrice(SCR_manager_ui.instance.GetWorkerStrengthCostText(), workerStrength);
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

    public float GetDeltaTimeModif()
    {
        return deltaTimeModif;
    }

    public void Sell(int resource)
    {
        money += resource; //Change Later
        SCR_manager_ui.instance.UpdateMoneyDisplay(money);
    }

    public float GetWorkerSpeed()
    {
        return workerSpeed.statCurrent;
    }

    public int GetWorkerStrength()
    {
        return workerStrength.statCurrent;
    }

    private bool CheckCost(int cost)
    {
        return (money >= cost);
    }
    private int CalculatePolynominalCost(statStruct stat) //Add multiplier pram later
    {
        return stat.initialPrice + stat.increaseRate * (int)Mathf.Pow(stat.n, 2);
    }
    private void Transaction(TextMeshProUGUI costDisplay, int cost, ref statStruct stat)
    {
        money -= cost;
        stat.n++;
        costDisplay.text = "$" + CalculatePolynominalCost(stat).ToString();
        stat.statCurrent++;
    }
    public void BuyWorker()
    {
        int cost = CalculatePolynominalCost(worker);
        if (CheckCost(cost))
        {
            shouldSpawnWorker = true; //I hate this

            Transaction(SCR_manager_ui.instance.GetWorkerCostText(), cost, ref worker);
        }
    }
    public void BuyWorkerStrength()
    {
        int cost = CalculatePolynominalCost(workerStrength);
        if (CheckCost(cost))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerStrengthCostText(), cost, ref workerStrength);
        }
    }
    public void BuyWorkerSpeed()
    {
        int cost = CalculatePolynominalCost(workerSpeed);
        if (CheckCost(cost))
        {
            Transaction(SCR_manager_ui.instance.GetWorkerSpeedCostText(), cost, ref workerSpeed);
        }
    }
}
