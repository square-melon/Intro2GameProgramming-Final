using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparky : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;

    private Dictionary<int, bool> HitEnemy;

    // Start is called before the first frame update
    void Start()
    {
        HitEnemy = new Dictionary<int, bool>();
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
