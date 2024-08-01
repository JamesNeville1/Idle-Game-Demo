using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile(CompileSynchronously = true)]
public partial class SCR_system_workers : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        //NativeArray<int> soldResource = new NativeArray<int>(1, Allocator.TempJob);
        var workerMoveJob = new WorkerMoveJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime * SCR_manager_main.instance.GetDeltaTimeModif() * (SCR_manager_main.instance.GetWorkerSpeed() + 1),
            sourceX = SCR_source.instance.transform.position.x,
            depositX = SCR_deposit.instance.transform.position.x,
            speed = SCR_manager_main.instance.GetWorkerSpeed(),
            carryCap = SCR_manager_main.instance.GetWorkerStrength(),
            soldResource = new NativeArray<int>(1, Allocator.TempJob),
            workerDistanceOffset = SCR_manager_main.instance.GetWorkerDistanceOffset()
        };
        JobHandle jobHandle = workerMoveJob.Schedule(this.Dependency);
        jobHandle.Complete();

        SCR_manager_main.instance.Sell(workerMoveJob.soldResource[0]);
    }

    [BurstCompile(CompileSynchronously = true)]
    public partial struct WorkerMoveJob : IJobEntity 
    {

        public float deltaTime;
        public float sourceX;
        public float depositX;
        public float speed;
        public int carryCap;
        public float workerDistanceOffset;

        public NativeArray<int> soldResource; 
        [BurstCompile]
        public void Execute(ref LocalTransform transform, ref SCR_component_worker worker)
        {
            float3 step = new float3(speed, 0, 0) * deltaTime; 
            if (worker.heldItem <= 0)
            {
                transform.Position += step; 

                if (transform.Position.x > sourceX - workerDistanceOffset)
                {
                    worker.heldItem = carryCap;  
                } 
            } 
            if (worker.heldItem > 0)
            {

                transform.Position -= step;

                if (transform.Position.x < depositX + workerDistanceOffset)
                {
                    soldResource[0] += worker.heldItem; 
                    worker.heldItem = 0;
                }
            }

            //Note for future James - Consider giving the worker a tag when they are holding something
        }
    }
}
