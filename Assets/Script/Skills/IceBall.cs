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
    private bool explode;
    void Start()
    {
        Origin = transform.position;
    }

    void Update()
    {
        if (!explode) {
            Ray ray = new Ray(transform.position, GetComponent<Rigidbody>().velocity);
            Debug.DrawRay(transform.position, GetComponent<Rigidbody>().velocity.normalized * 0.5f, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.5f)) {
                Debug.Log("IceBall: " + hit.collider.transform.root.name);
                if (hit.collider.transform.root.CompareTag("Enemy") || hit.collider.transform.root.CompareTag("Monster")) {
                    explode = true;
                    StartCoroutine(Explode(ChangeToEnemyTrans(hit.transform.root)));
                }
            }
            if (Vector3.Distance(transform.position, Origin) >= ExplodeDistance) {
                explode = true;
                StartCoroutine(Explode(null));
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (!explode) {
            if (other.transform.root.CompareTag("Enemy") || other.transform.root.CompareTag("Monster")) {
                explode = true;
                StartCoroutine(Explode(ChangeToEnemyTrans(other.transform.root)));
            } else {
                Destroy(gameObject);
            }
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
        if (Enemy.CompareTag("Enemy") && Enemy.GetComponent<FireDemon>() == null) return Enemy;
        for (int j = Enemy.transform.childCount - 1; j >= 0; j--) {
            if (Enemy.transform.GetChild(j).gameObject.activeSelf && Enemy.transform.GetChild(j).CompareTag("Enemy")) {
                return Enemy.transform.GetChild(j).gameObject;
            }
        }
        return null;
    }

    Transform ChangeToEnemyTrans(Transform Enemy) {
        if (Enemy.CompareTag("Enemy") && Enemy.gameObject.GetComponent<FireDemon>() == null) return Enemy;
        for (int j = Enemy.childCount - 1; j >= 0; j--) {
            if (Enemy.GetChild(j).gameObject.activeSelf && Enemy.GetChild(j).CompareTag("Enemy")) {
                return Enemy.GetChild(j);
            }
        }
        return null;
    }
}
