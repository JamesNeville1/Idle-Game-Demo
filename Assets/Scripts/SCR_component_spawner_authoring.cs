using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_component_spawner_authoring : MonoBehaviour
{
    public GameObject workerPrefab; // Worker *Game Object*

    public class baker : Baker<SCR_component_spawner_authoring>
    {
        public override void Bake(SCR_component_spawner_authoring authoring)
        {
            SCR_component_spawner spawner = default;
            spawner.workerEntity = GetEntity(authoring.workerPrefab, TransformUsageFlags.None);

            Entity ent = GetEntity(TransformUsageFlags.None);

            AddComponent(ent, spawner);
        }
    }
}

public struct SCR_component_spawner : IComponentData
{
    public Entity workerEntity;
}
