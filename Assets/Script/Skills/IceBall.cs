using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    
    [Header("Settings")]
    public float ExplodeDistance;
    public float ExplodeWait;
    public float ExplosionEffectWait;
    public GameObject FrostBeam;
    public GameObject IceBallExplosion;

    private Vector3 Origin;
    void Start()
    {
        Origin = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Origin) >= ExplodeDistance) {
            StartCoroutine(Explode(null));
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.root.CompareTag("Enemy") || other.transform.root.CompareTag("Monster")) {
            StartCoroutine(Explode(ChangeToEnemyTrans(other.transform.root)));
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator Explode(Transform Enemy) {
        Vector3 dir1 = new Vector3(0, 0, 0);
        Vector3 dir2 = new Vector3(0, 90, 0);
        Vector3 dir3 = new Vector3(0, 180, 0);
        Vector3 dir4 = new Vector3(0, 270, 0);
        Vector3 Target = transform.forward;
        yield return new WaitForSeconds(ExplodeWait);
        Vector3 posi = transform.position;
        if (Enemy != null) {
            posi = Enemy.position;
        }
        posi.y = DataManager.Instance.PlayerPos.y;
        GameObject FrostPre1 = Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target) * Quaternion.Euler(dir1));
        GameObject FrostPre2 = Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target) * Quaternion.Euler(dir2));
        GameObject FrostPre3 = Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target) * Quaternion.Euler(dir3));
        GameObject FrostPre4 = Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target) * Quaternion.Euler(dir4));
        GameObject ExplosionEffect = Instantiate(IceBallExplosion, posi + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(gameObject);
    }

    GameObject ChangeToEnemy(GameObject Enemy) {
        if (Enemy.CompareTag("Enemy")) return Enemy;
        for (int j = Enemy.transform.childCount - 1; j >= 0; j--) {
            if (Enemy.transform.GetChild(j).CompareTag("Enemy")) {
                return Enemy.transform.GetChild(j).gameObject;
            }
        }
        return null;
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
