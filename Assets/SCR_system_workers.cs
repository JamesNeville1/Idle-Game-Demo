using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SCR_system_workers : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, worker) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SCR_component_worker>>())
        {
            float3 pos = transform.ValueRO.Position;
            pos += new float3(1, 0, 0) * Time.deltaTime;
            transform.ValueRW.Position = pos;

            //Note for future James - Consider giving the worker a tag when they are holding something
        }
    }
}
