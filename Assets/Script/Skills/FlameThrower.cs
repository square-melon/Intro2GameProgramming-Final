using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float RayRadius;
    public float RayLength;

    void Start()
    {
        Destroy(gameObject, ExistTime);
    }

    void Update()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, RayRadius, transform.forward, RayLength);
        foreach (var obj in hit) {
            if (obj.transform.root.CompareTag("Enemy") || obj.transform.root.CompareTag("Monster")) {
                if (BurnerManager.Instance != null) {
                    BurnerManager.Instance.BurnOn(obj.transform.root);
                }
            }
        }
    }
}
