using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_source : MonoBehaviour
{
    public static SCR_source instance { private set; get; }
    private void Awake()
    {
        instance = this;
    }
    public void Take(ref SCR_component_worker worker)
    {
        worker.heldItem = SCR_manager_main.instance.GetWorkerCarryingCapacity();
    }
}
