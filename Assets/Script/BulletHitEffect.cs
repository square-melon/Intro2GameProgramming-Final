using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{

    [Header("Settings")]
    public float RemainTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, RemainTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
