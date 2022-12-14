using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float Damage;
    public float Radius;
    public float Dis;

    private Dictionary<int, int> enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new Dictionary<int, int>();
        Destroy(gameObject, ExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, Radius, Vector3.up, Dis);
        foreach (var obj in hit) {
            if (obj.collider.CompareTag("Enemy")) {
                int hash = obj.transform.root.GetHashCode();
                if (!enemies.ContainsKey(hash)) {
                    DataManager.Instance.takedamage(obj.transform.root, Damage);
                    enemies.Add(hash, 1);
                }
            }
        }
    }
}
