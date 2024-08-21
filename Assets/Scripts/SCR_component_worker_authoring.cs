using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_component_worker_authoring : MonoBehaviour
{
    private class baker : Baker<SCR_component_worker_authoring>
    {
        public override void Bake(SCR_component_worker_authoring authoring)
        {
            Entity ent = GetEntity(TransformUsageFlags.None);

            AddComponent(ent, new SCR_component_worker {heldItem = 0});
        }
    }
}

public struct SCR_component_worker : IComponentData
{
    public int heldItem;
}