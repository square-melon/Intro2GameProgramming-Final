using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparky : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float AreaDamageTime;
    public float scaling;
    public float damage;

    private Dictionary<int, bool> HitEnemy;
    private ParticleSystem particle;
    bool FirstHit;

    // Start is called before the first frame update
    void Start()
    {
        FirstHit = false;
        HitEnemy = new Dictionary<int, bool>();
        particle = GetComponent<ParticleSystem>();
        Invoke("DestroyMe", ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyMe() {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            if (!FirstHit) {
                FirstHit = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                StartCoroutine(AreaDamage());
                CancelInvoke("DestroyMe");
            }
            int hash = other.gameObject.transform.root.GetHashCode();
            if (HitEnemy.ContainsKey(hash) == false) {
                HitEnemy.Add(hash, true);
                takedamage(other.gameObject.transform.root, damage);
            }
        }
    }

    IEnumerator AreaDamage() {
        float dur = 0;
        while (dur < AreaDamageTime) {
            dur += Time.deltaTime;
            var ma = particle.main;
            Color Into = ma.startColor.color;
            if (Into.a <= 0.2f) {
                Into.a -= 0.01f;
                ma.startColor = Into;
            } else {
                Into.a = 0f;
                ma.startColor = Color.Lerp(ma.startColor.color, Into, Time.deltaTime * 3);
            }
            transform.localScale *= 1 + scaling;
            yield return null;
        }
        DestroyMe();
    }

    void takedamage(Transform enemy, float damage) {
        Scene2Enemy e1 = enemy.GetComponent<Scene2Enemy>();
        enemyScript e2 = enemy.GetComponent<enemyScript>();
        Zombie3script ee2 = enemy.GetComponent<Zombie3script>();
        Wizard e3 = enemy.GetComponent<Wizard>();
        Scene2Boss ee = enemy.GetComponent<Scene2Boss>();
        if (e1)
            e1.Damage();
        else if (e2)
            e2.Damage();
        // else if (e3)
        //     e3.Damage();
        else if (ee)
            ee.Damage();
        else if (ee2)
            ee2.Damage();
    }
}
