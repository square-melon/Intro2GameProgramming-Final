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

    private Dictionary<int, bool> HitEnemy;
    private float Scaling;

    // Start is called before the first frame update
    void Start()
    {
        HitEnemy = new Dictionary<int, bool>();
        Scaling = DataManager.Instance.Scaling;
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, RayRadius * Scaling, transform.forward, RayLength * Scaling);
        // Debug.DrawRay(transform.position, transform.forward * RayLength);
        foreach (var obj in hit) {
            if (obj.collider.CompareTag("Enemy")) {
                int hash = obj.collider.transform.root.GetHashCode();
                if (HitEnemy.ContainsKey(hash) == false) {
                    HitEnemy.Add(hash, true);
                    GameObject Hit = Instantiate(HitEffect, obj.point, Quaternion.identity);
                    Hit.transform.localScale = new Vector3(Scaling, Scaling, Scaling);
                    takedamage(obj.transform.root);
                }
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
