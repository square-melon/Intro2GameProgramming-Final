using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCreate : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public GameObject HitEffect;
    public float RayLength;
    public float RayRadius;

    private bool ExplodeCreated;
    
    void Start()
    {
        Destroy(gameObject, ExistTime);
        ExplodeCreated = false;
    }
    
    void Update()
    {
        // DetectShootOn();
    }

    void DetectShootOn() {
        // Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * RayLength);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, RayRadius, transform.forward, out hit, RayLength)) {
            if (hit.collider.CompareTag("Enemy")) {
                if (ExplodeCreated == false) {
                    ExplodeCreated = true;
                    Instantiate(HitEffect, hit.point, Quaternion.identity);
                }
                //takedamage(hit.transform);
            }
        }
    }

    // void takedamage(Transform enemy) {
    //     Scene2Enemy e1 = enemy.GetComponent<Scene2Enemy>();
    //     enemyScript e2 = enemy.GetComponent<enemyScript>();
    //     Zombie3script ee2 = enemy.GetComponent<Zombie3script>();
    //     Wizard e3 = enemy.GetComponent<Wizard>();
    //     Scene2Boss ee = enemy.GetComponent<Scene2Boss>();
    //     if (e1)
    //         e1.Damage();
    //     else if (e2)
    //         e2.Damage();
    //     else if (e3)
    //         e3.Damage();
    //     else if (ee)
    //         ee.Damage();
    //     else if (ee2)
    //         ee2.Damage();
    //  }

    // void OnCollisionEnter(Collision other) {
    //     if (!other.collider.CompareTag("Player")) {
    //         if (other.collider.CompareTag("Enemy")) {
    //             Instantiate(LightningEffect, other.contacts[0].point, Quaternion.identity);
    //             takedamage(other.collider.transform);
    //         }
    //         Destroy(gameObject);
    //     }
    // }
}
