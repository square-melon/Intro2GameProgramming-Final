using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDisable : MonoBehaviour
{
    [Header("Settings")]
    public float ExistTime;

    void Start()
    {
        Destroy(gameObject, ExistTime);
    }

    void Update()
    {
        
    }
}
