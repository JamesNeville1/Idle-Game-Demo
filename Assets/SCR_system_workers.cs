using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SCR_system_workers : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        WorkerMoveJob workerMoveJob = new WorkerMoveJob 
        {
            deltaTime = SystemAPI.Time.DeltaTime * 15,
            sourceX = SCR_source.instance.transform.position.x,
            depositX = SCR_deposit.instance.transform.position.x,
            workerSpeed = SCR_manager_main.instance.GetWorkerSpeed()
        };
        workerMoveJob.Schedule();
    }


    public partial struct WorkerMoveJob : IJobEntity
    {

        public float deltaTime;
        public float sourceX;
        public float depositX;
        public float workerSpeed;

        public void Execute(ref LocalTransform transform, ref SCR_component_worker worker)
        {
            float3 step = new float3(workerSpeed, 0, 0) * deltaTime;
            if (worker.heldItem <= 0)
            {
                transform.Position += step;

                if (math.abs(sourceX - transform.Position.x) <= 2.5f)
                {
                    SCR_source.instance.Take(ref worker);
                }
            }
            if (worker.heldItem > 0)
            {

                transform.Position -= step;

                if (math.abs(depositX - transform.Position.x) <= 2.5f)
                {
                    SCR_deposit.instance.Give(ref worker);
                }
            }

            //Note for future James - Consider giving the worker a tag when they are holding something
        }
    }
}
