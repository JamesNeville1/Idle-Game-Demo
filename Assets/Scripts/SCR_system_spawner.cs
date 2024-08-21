using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial struct SCR_system_spawner : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SCR_manager_main.instance.shouldSpawnWorker) //Should spawn worker?
        {
            foreach (var spawner in SystemAPI.Query<RefRO<SCR_component_spawner>>().WithEntityAccess()) //Get spawner
            {
                var ecb =
                    SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

                //Spawn
                Entity worker = ecb.Instantiate(spawner.Item1.ValueRO.workerEntity);
                ecb.SetComponent(worker, new LocalTransform()
                {
                    Position = new float3(UnityEngine.Random.Range(SCR_source.instance.transform.position.x, SCR_deposit.instance.transform.position.x), 0, 0),
                    Scale = 1,
                    Rotation = new quaternion(0, 0, 0, 1)
                });

                Debug.Log("Log: Worker Summoned");

                //Only do once
                SCR_manager_main.instance.shouldSpawnWorker = false;
            }
        }
    }
}
