using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
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
                for (int i = 0; i < 14500; i++)
                {
                    var ecb =
                        SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

                    Entity worker = ecb.Instantiate(spawner.Item1.ValueRO.workerEntity);
                    ecb.SetComponent(worker, new LocalTransform()
                    {
                        Position = new float3(UnityEngine.Random.Range(SCR_source.instance.transform.position.x, SCR_deposit.instance.transform.position.x), 0, 0),
                        Scale = 1,
                        Rotation = new quaternion(0, 0, 0, 1)
                    });
                    //ecb.AddComponent(worker, new SCR_component_worker_collecting { });

                    Debug.Log("Log: Worker Summoned");
                    SCR_manager_main.instance.shouldSpawnWorker = false;
                }
            }
        }
    }
}
