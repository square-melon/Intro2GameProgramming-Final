using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedEffect : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
