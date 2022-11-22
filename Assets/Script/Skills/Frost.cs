using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float RayRadius;
    public float RayLength;
    public float Damage;
    public float FrozenTime;
    public GameObject HitEffect;

    private bool ExplodeCreated;

    // Start is called before the first frame update
    void Start()
    {
        ExplodeCreated = false;
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * RayLength);
        if (Physics.SphereCast(transform.position, RayRadius, transform.forward, out hit, RayLength)) {
            if (hit.collider.CompareTag("Enemy")) {
                if (ExplodeCreated == false) {
                    ExplodeCreated = true;
                    Instantiate(HitEffect, hit.point, Quaternion.identity);
                }
                takedamage(hit.transform);
            }
        }
    }

    void takedamage(Transform enemy) {
        Scene2Enemy e1 = enemy.GetComponent<Scene2Enemy>();
        enemyScript e2 = enemy.GetComponent<enemyScript>();
        Zombie3script ee2 = enemy.GetComponent<Zombie3script>();
        Wizard e3 = enemy.GetComponent<Wizard>();
        Scene2Boss ee = enemy.GetComponent<Scene2Boss>();
        if (e1)
            e1.Damage();
        else if (e2)
            e2.Damage();
        else if (e3)
            e3.Damage();
        else if (ee)
            ee.Damage();
        else if (ee2)
            ee2.Damage();
    }
}
