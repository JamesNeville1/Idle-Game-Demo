using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_source : MonoBehaviour
{
    public void Take(SCR_component_worker worker)
    {
        worker.heldItem = SCR_manager_main.instance.GetWorkerCarryingCapacity();
    }
}
