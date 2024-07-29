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
}
