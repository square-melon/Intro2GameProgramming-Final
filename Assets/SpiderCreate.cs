using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCreate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spider;
    private GameObject spiderprefab;
    public float timetospawn;
    private int flag = 0;
    private float health = 100;
    private int dead = 0;
    public GameObject spark;
    private GameObject sparkprefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flag == 0) {
            Invoke("Create",timetospawn);
            flag = 1;
        }
        Death();
        
    }
    void Create() {
        
        spiderprefab = Instantiate(spider,transform.position,Quaternion.identity);
        flag = 0;
        
        
        
    }
    public void Damage(float damage) {
        //print(damage);
        
        health -= damage;
        Debug.Log(health);
    }
    void Death() {
        if(dead == 0) {
            if(health <= 0.0f) {
                
                Invoke("Spark",0.1f);
                Invoke("disappear",2.5f);
                //agent.SetDestination(transform.position);
                dead = 1;
            }
        }
    }
    void Spark() {
        Vector3 po = new Vector3(transform.position.x, transform.position.y+3.0f,transform.position.z);
        sparkprefab = Instantiate(spark,po,Quaternion.identity);
        Destroy(sparkprefab,2.0f);
    }
    void disappear() {
        gameObject.SetActive(false);
    }
}
