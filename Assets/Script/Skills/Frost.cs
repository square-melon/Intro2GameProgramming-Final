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
                    DataManager.Instance.takedamage(obj.transform.root, Damage);
                }
            }
        }
    }
}
