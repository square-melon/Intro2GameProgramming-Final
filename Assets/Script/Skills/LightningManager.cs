using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    public static LightningManager Instance { get; private set; }

    public Dictionary<int, int> Lightning { get; private set; }
    public float BasicDamage;
    public float AdditionalDamage;

    private void Awake() {
        Lightning = new Dictionary<int, int>();
    }

    public void HitOn(GameObject obj) {
        int hash = obj.GetHashCode();
        if (Lightning.ContainsKey(hash)) {
            Lightning[hash]++;
        } else {
            Lightning.Add(hash, 1);
        }
        takedamage(obj.transform, BasicDamage + (Lightning[hash] - 1) * AdditionalDamage);
    }

    private void takedamage(Transform enemy, float damage) {
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
