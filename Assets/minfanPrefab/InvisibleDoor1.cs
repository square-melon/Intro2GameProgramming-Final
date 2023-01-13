using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDoor1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Obstacle1;
    public GameObject door1;
    private Animator Door1Anim;
    public float radius;
    void Start()
    {
        Door1Anim = door1.GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     RaycastHit[] hit = Physics.SphereCastAll(transform.position,radius,transform.forward,0);
    //     foreach(var obj in hit){
    //         if(obj.collider.CompareTag("Player")){
    //             print("open");
    //             Invoke("Opendoor",4.0f);
    //         }
    //     }
    // }
    void OnTriggerEnter(Collider other){
        print("aa");
        if(other.tag=="Player"){
            Invoke("Opendoor",4.0f);
            Invoke("RemoveObstacle",5.0f);
        }
    }
    void Opendoor(){
        
        Door1Anim.SetTrigger("Open");
    }
    void RemoveObstacle(){
        Obstacle1.SetActive(false);
    }
}
