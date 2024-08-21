using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class SCR_system_workers : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var workerMoveJob = new WorkerMoveJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime * SCR_manager_main.instance.GetDeltaTimeModif() * (SCR_manager_main.instance.GetWorkerSpeed() + 1),
            sourceX = SCR_source.instance.transform.position.x,
            depositX = SCR_deposit.instance.transform.position.x,
            speed = SCR_manager_main.instance.GetWorkerSpeed(),
            carryCap = SCR_manager_main.instance.GetWorkerStrength(),
            soldResource = new NativeArray<int>(1, Allocator.TempJob),
            pickedUp = new NativeArray<bool>(1, Allocator.TempJob),
            workerDistanceOffset = SCR_manager_main.instance.GetWorkerDistanceOffset()
        };

        //Schedule job, wait until complete before continuing
        JobHandle jobHandle = workerMoveJob.Schedule(this.Dependency);
        jobHandle.Complete();

        //This cant be done within the job, so we check here
        if (workerMoveJob.soldResource[0] > 0) SCR_manager_main.instance.Sell(workerMoveJob.soldResource[0]);
        else if (workerMoveJob.pickedUp[0] == true) SCR_manager_audio.instance.PlayRandomEffect("RESOURCE_PICKUP");
        
        //Free up memory
        workerMoveJob.soldResource.Dispose();
        workerMoveJob.pickedUp.Dispose();
    }

    [BurstCompile]
    public partial struct WorkerMoveJob : IJobEntity 
    {

        public float deltaTime; //Feed delta time into job
        public float sourceX; //Where the resource source is
        public float depositX; //Where the deposit is
        public float speed; //How fast the workers go
        public int carryCap; //How much the workers carry
        public float workerDistanceOffset; //How far they can be from the deposit and source

        public NativeArray<int> soldResource; //How much has been sold
        public NativeArray<bool> pickedUp; //Has a resource been pickedup
        [BurstCompile]
        public void Execute(ref LocalTransform transform, ref SCR_component_worker worker)
        {
            float3 step = new float3(speed, 0, 0) * deltaTime; //The step of a frame

            if (worker.heldItem <= 0) //Move Towards Supply
            {
                transform.Position += step;
                if (transform.Position.x > sourceX - workerDistanceOffset) //Pickup resource and move towards deposit
                {
                    pickedUp[0] = true;
                    worker.heldItem = carryCap;
                    transform.Rotation.value.y = 1;
                    transform.Position.z = -1f;
                } 
            } 

            else if (worker.heldItem > 0) //Move Towards Deposit
            {

                transform.Position -= step;

                if (transform.Position.x < depositX + workerDistanceOffset) //Sell resource and move towards source
                {
                    soldResource[0] += worker.heldItem; 
                    worker.heldItem = 0;
                    transform.Rotation.value.y = 0;
                    transform.Position.z = 0;
                }
            }
        }
    }
}
