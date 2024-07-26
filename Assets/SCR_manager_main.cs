using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_manager_main : MonoBehaviour
{
    [SerializeField] private int money;
    [SerializeField] private float workerSpeed = 1;
    [SerializeField] private int workerCarryingCapacity = 1;

    private EntityManager entityManager;
    private SCR_component_spawner spawner;

    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform haulLineBegin;
    [SerializeField] private Transform haulLineEnd;

    public bool shouldSpawnWorker;

    public static SCR_manager_main instance { get; private set; }
    private void Awake()
    {
        instance = this;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public int GetMoney()
    {
        return money;
    }

    public void Sell(int resource)
    {
        money += resource; //Change Later
    }

    public float GetWorkerSpeed()
    {
        return workerSpeed;
    }

    public int GetWorkerCarryingCapacity()
    {
        return workerCarryingCapacity;
    }

    public void TriggerSpawnWorkerEntity()
    {
        //Vector2 pos = new Vector2 (Random.Range(haulLineBegin.position.x, haulLineEnd.position.x), 0);
        //Instantiate(workerPrefab, pos, Quaternion.identity);

        //Entity worker = entityManager.Instantiate(workerRef);

        //entityManager.RemoveComponent<Disabled>(worker);

        //GetEntity(workerPrefab);

        //var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(State.)

        //entityManager.CreateEntity(spawner.workerEntity);

        shouldSpawnWorker = true; //I hate this
    }
}
