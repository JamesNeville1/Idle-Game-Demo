using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_manager_main : MonoBehaviour
{
    private int money;
    private float workerSpeed;
    private int workerCarryingCapacity;

    private EntityManager entityManager;

    [SerializeField] private Entity workerRef;
    [SerializeField] private Transform haulLineBegin;
    [SerializeField] private Transform haulLineEnd;

    public static SCR_manager_main instance { get; private set; }
    private void Awake()
    {
        instance = this;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        var entities = entityManager.GetAllEntities();
        foreach (var entity in entities)
        {
            if (entityManager.HasComponent<SCR_component_worker>(entity) == true)
            {
                workerRef = entity;
                break;
            }
        }

        print(workerRef);
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

    public void CreateWorkerEntity()
    {
        //Vector2 pos = new Vector2 (Random.Range(haulLineBegin.position.x, haulLineEnd.position.x), 0);
        //Instantiate(workerPrefab, pos, Quaternion.identity);

        Entity worker = entityManager.Instantiate(workerRef);

        entityManager.RemoveComponent<Disabled>(worker);
    }
}
