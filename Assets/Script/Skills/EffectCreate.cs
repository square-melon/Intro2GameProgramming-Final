using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCreate : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;

    void Start()
    {
        Destroy(gameObject, ExistTime);
    }
}
