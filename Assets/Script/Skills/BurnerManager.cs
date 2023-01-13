using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerManager : MonoBehaviour
{
    public static BurnerManager Instance { get; private set; }

    public Dictionary<int, int> Burning { get; private set; }
    public List<GameObject> BurningObj;
    public int MaxBurnTime;
    public float BurnFreq;
    public float BurnDamage;
    public float DestroyTime;
    public float HurtingDestroyTime;
    public GameObject BurnEffect;
    public GameObject BurnHurting;

    int OnBurn;
    private void Awake() {
        Instance = this;
        Burning = new Dictionary<int, int>();
        BurningObj = new List<GameObject>();
        OnBurn = 0;
        StartCoroutine(DestroyThis());
    }

    public void BurnOn(Transform obj) {
        obj = ChangeToEnemyTrans(obj);
        int hash = obj.GetHashCode();
        if (Burning.ContainsKey(hash))
            return;
        BurningObj.Add(obj.gameObject);
        Burning.Add(hash, 0);
        OnBurn++;
        StartCoroutine(Burn(obj));
    }

    private Transform ChangeToEnemyTrans(Transform Enemy) {
        Enemy = Enemy.root;
        if (Enemy.gameObject.GetComponent<FireDemon>() != null) return Enemy;
        if (Enemy.CompareTag("Enemy")) return Enemy;
        if (Enemy.CompareTag("Player")) return Enemy;
        for (int j = Enemy.childCount - 1; j >= 0; j--) {
            if (Enemy.GetChild(j).gameObject.activeSelf && Enemy.GetChild(j).CompareTag("Enemy")) {
                return Enemy.GetChild(j);
            }
        }
        return null;
    }

    IEnumerator Burn(Transform enemy) {
        int hash = enemy.GetHashCode();
        GameObject BurnEffectPrefab = Instantiate(BurnEffect, enemy.position, Quaternion.identity);
        while (Burning[hash] < MaxBurnTime) {
            float dur = 0;
            while (dur < BurnFreq) {
                BurnEffectPrefab.transform.position = enemy.position;
                dur += Time.deltaTime;
                yield return null;
            }
            Burning[hash]++;
            GameObject BurnHurtingPrefab = Instantiate(BurnHurting, enemy.position + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
            Destroy(BurnHurtingPrefab, HurtingDestroyTime);
            DataManager.Instance.takedamage(enemy, BurnDamage);
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(BurnEffectPrefab);
        OnBurn--;
    }

    IEnumerator DestroyThis() {
        yield return new WaitForSeconds(DestroyTime);
        while (true) {
            if (OnBurn == 0)
                Destroy(gameObject);
            yield return null;
        }
    }
}
