using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBulletCreate : MonoBehaviour
{
    [Header("Settings")]
    public float ExistTime;
    public float RayLength;
    public float RayRadius;
    public float Angle;

    private float Scaling;
    private Physics physics;
    public Dictionary<int, bool> HitEnemy;

    void Start()
    {
        Scaling = DataManager.Instance.Scaling;
        HitEnemy = new Dictionary<int, bool>();
        Destroy(gameObject, ExistTime);
    }
    
    void Update()
    {
        DetectShootOn();
    }

    void DetectShootOn() {
        RaycastHit[] hit = physics.ConeCastAll(transform.position, RayRadius*Scaling, transform.forward, RayLength*Scaling, Angle);
        foreach (var obj in hit) {
            if (obj.collider.gameObject.CompareTag("Enemy")) {
                int hash = obj.collider.transform.root.GetHashCode();
                if (!HitEnemy.ContainsKey(hash)) {
                    if (LightningManager.Instance != null)
                        LightningManager.Instance.HitOn(obj.collider.transform.root);
                    HitEnemy.Add(hash, true);
                }
            }
        }
    }

    // void OnCollisionEnter(Collision other) {
    //     if (!other.collider.CompareTag("Player")) {
    //         if (other.collider.CompareTag("Enemy")) {
    //             if (LightningManager.Instance != null)
    //                 LightningManager.Instance.HitOn(other.collider.gameObject);
    //         }
    //         Destroy(gameObject);
    //     }
    // }
}
