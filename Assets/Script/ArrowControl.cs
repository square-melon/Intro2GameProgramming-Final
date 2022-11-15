using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Player") {
            Destroy(gameObject);
            //扣寫
        }
        
    }
    void DetectShoot() {
        Ray ray = new Ray(transform.position, GetComponent<Rigidbody>().velocity);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.3f)) {
            if (hit.collider.CompareTag("Player")) {
                //gameController._PlayerBulletHitOn(hit.collider.gameObject);
                //takedamage(hit.transform);
                Debug.Log(hit.collider.name);
                //Destroy(gameObject);
                
            }
        }
    }
}
