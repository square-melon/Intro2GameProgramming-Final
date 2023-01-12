using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingThunder : MonoBehaviour
{
    [Header("Settings")]
    public float ExistTime;
    public float RayRadius;
    public float RayLength;
    private Dictionary<int, bool> HitEnemy;

    private float Scaling;
    // Start is called before the first frame update
    void Start()
    {
        Scaling = DataManager.Instance.Scaling;
        HitEnemy = new Dictionary<int, bool>();
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, RayRadius * Scaling, Vector3.up, RayLength * Scaling);
        foreach (var obj in hit) {
            if (obj.transform.root.CompareTag("Enemy") || obj.transform.root.CompareTag("Monster") || obj.transform.root.CompareTag("Player")) {
                int hash = obj.transform.root.GetHashCode();
                if (HitEnemy.ContainsKey(hash) == false) {
                    HitEnemy.Add(hash, true);
                    if (LightningManager.Instance != null)
                        LightningManager.Instance.HitOn(obj.transform.root);
                }
            }
        }
    }
}
