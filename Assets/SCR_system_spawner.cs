using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct SCR_system_spawner : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SCR_manager_main.instance.shouldSpawnWorker)
        {
            foreach (var spawner in SystemAPI.Query<RefRO<SCR_component_spawner>>().WithEntityAccess())
            {
                var ecb =
                    SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

                Entity worker = ecb.Instantiate(spawner.Item1.ValueRO.workerEntity);
                ecb.AddComponent(worker, new SCR_component_worker_collecting { });

                Debug.Log("Log: Worker Summoned");
                SCR_manager_main.instance.shouldSpawnWorker = false;
            }
        }
    }
}