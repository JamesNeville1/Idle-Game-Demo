using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_deposit : MonoBehaviour
{
    public void Give(SCR_component_worker worker)
    {
        int resource = worker.heldItem;
        worker.heldItem = 0;

        SCR_manager_main.instance.Sell(resource);
    }
}
