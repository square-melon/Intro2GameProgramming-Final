using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreate : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public float Damage = 30.0f;
    public float RayRadius;
    public GameObject ExplodeEffect;
    public GameObject damagetext;
    void Start()
    {
        Destroy(gameObject, ExistTime);
    }
    
    void Update()
    {
        DetectShootOn();
    }

    void DetectShootOn() {
        Ray ray = new Ray(transform.position, GetComponent<Rigidbody>().velocity);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, RayRadius, GetComponent<Rigidbody>().velocity, out hit, 0.5f)) {
            if (hit.collider.CompareTag("Enemy")) {
                Instantiate(ExplodeEffect, hit.point, Quaternion.identity);
                DataManager.Instance.takedamage(hit.transform.root, Damage);
                Destroy(gameObject);
                
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (!other.collider.CompareTag("Player")) {
            if (other.collider.CompareTag("Enemy")) {
                Instantiate(ExplodeEffect, other.contacts[0].point, Quaternion.identity);
                DataManager.Instance.takedamage(other.collider.transform.root, Damage);
            }
        }
        Destroy(gameObject);
    }
    void ShowDamage() {
        var go = Instantiate(damagetext, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = "30";
    }
}
