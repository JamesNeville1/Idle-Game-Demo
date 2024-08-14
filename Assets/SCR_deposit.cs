using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_deposit : MonoBehaviour
{
    public static SCR_deposit instance { private set; get; }
    private void Awake()
    {
        instance = this;
    }
}
