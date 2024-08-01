using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SCR_source : MonoBehaviour
{
    public static SCR_source instance { private set; get; }

    private bool canManualClick;
    [SerializeField] private float timeBetweenManualClick;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canManualClick = true;
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && canManualClick)
        {
            SCR_manager_main.instance.Sell(SCR_manager_main.instance.GetWorkerStrength());
            StartCoroutine(ManualClickCooldown());
            //Play SFX
        }
    }

    private IEnumerator ManualClickCooldown()
    {
        canManualClick = false;
        yield return new WaitForSeconds(timeBetweenManualClick);
        canManualClick = true;
    }
}
