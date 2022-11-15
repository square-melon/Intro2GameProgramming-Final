using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreate : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public GameObject GameControllerObj;
    public GameObject ExplodeEffect;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, ExistTime);
        gameController = GameControllerObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    
    void Update()
    {
        DetectShootOn();
    }

    void DetectShootOn() {
        Ray ray = new Ray(transform.position, GetComponent<Rigidbody>().velocity);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.5f)) {
            if (hit.collider.CompareTag("Enemy")) {
                gameController._PlayerBulletHitOn(hit.collider.gameObject);
                Instantiate(ExplodeEffect, hit.point, Quaternion.identity);
                //print("yes" + i);
                Debug.Log(hit.collider.name);
                takedamage(hit.transform);
                //i++;
                Destroy(gameObject);
            }
        }
    }
    void takedamage(Transform enemy) {
        Scene2Enemy e1 = enemy.GetComponent<Scene2Enemy>();
        enemyScript e2 = enemy.GetComponent<enemyScript>();
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

    }
    void OnCollisionEnter(Collision other) {
        if (!other.collider.CompareTag("Player")) {
            if (other.collider.CompareTag("Enemy")) {
                Instantiate(ExplodeEffect, other.contacts[0].point, Quaternion.identity);
                takedamage(other.collider.transform);
            }
            Destroy(gameObject);
        }
        // if(other.gameObject == "Enemy") {
        //     takedamage(other.transform);
        // }
    }
}
