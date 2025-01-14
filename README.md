# Idle Game Demo (Using ECS & Unity D.O.T.S)

### This project had the goal of learning the main elements of Unity D.O.T.S, especially ECS.

(My code can be found [here](https://github.com/JamesNeville1/Idle-Game-Demo/tree/main/Assets/Scripts))

ECS - Entity Component System is a far more optimal, all be it more challenging framework. The main principles are:
- Entities - Are complete templates, all they are. They populate the game
- Component - Are the components that entities use
- System - The logic that updates the components.

ECS is a data-oriented design, rather than object-oriented like the standard monobehaviour in Unity.
<br /><br />
Unity Manual - "The (component) data associated with with your entities, but organized by the data itself rather than by entity."
<br /><br />

Here is my use of BurstCompile, which is an attribute that converts your code into fast machine code. I also use IJobEntity which is a way of using multi-threading in Unity. The worker then moves back and forth between the deposit and the source.
```
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
```

This was a good learning experience, I plan to use ECS again in future projects as this seems to be Unity's main focus at this current moment in time.
