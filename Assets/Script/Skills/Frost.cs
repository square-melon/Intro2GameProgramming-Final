using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float halfx;
    public float halfy;
    public float halfz;
    public float RayLength;
    public float Damage;
    public float FrozenTime;
    public float FrozenRate;
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
        Vector3 center = transform.position + transform.forward.normalized * halfz + new Vector3(0, halfy, 0);
        RaycastHit[] hit = Physics.BoxCastAll(center, new Vector3(halfx, halfy, halfz), transform.forward, transform.rotation, RayLength);
        // Debug.DrawRay(transform.position, transform.forward * RayLength);
        foreach (var obj in hit) {
            if (obj.transform.root.CompareTag("Enemy") || obj.transform.root.CompareTag("Monster")) {
                int hash = obj.transform.root.GetHashCode();
                if (HitEnemy.ContainsKey(hash) == false) {
                    HitEnemy.Add(hash, true);
                    Transform objTrans = ChangeToEnemyTrans(obj.transform.root);
                    GameObject Hit = Instantiate(HitEffect, objTrans.position, Quaternion.identity);
                    Destroy(Hit, FrozenTime+0.2f);
                    DataManager.Instance.takedamage(objTrans, Damage);
                    StartCoroutine(Frozen(obj.transform.root.gameObject));
                }
            }
        }
    }

    IEnumerator Frozen(GameObject Enemy) {
        if (Enemy.TryGetComponent(out UnityEngine.AI.NavMeshAgent nav)) {
            float Ori = nav.speed;
            nav.speed = Ori * (1 - FrozenRate * 0.01f);
            yield return new WaitForSeconds(FrozenTime);
            nav.speed = Ori;
        }
    }

    Transform ChangeToEnemyTrans(Transform Enemy) {
        if (Enemy.CompareTag("Enemy")) return Enemy;
        for (int j = Enemy.childCount - 1; j >= 0; j--) {
            if (Enemy.GetChild(j).CompareTag("Enemy")) {
                return Enemy.GetChild(j);
            }
        }
        return null;
    }
}
