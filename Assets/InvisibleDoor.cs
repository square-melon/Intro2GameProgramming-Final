using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Obstacle1;
    public GameObject door1;
    public GameObject Obstacle2;
    public GameObject door2;
    public GameObject Obstacle3;
    public GameObject door3;
    public GameObject Obstacle4;
    public GameObject door4;
    public GameObject Obstacle5;
    public GameObject door5;
    public GameObject Obstacle6;
    public GameObject door6;
    public GameObject Obstacle7;
    public GameObject door7;
    public GameObject Obstacle8;
    public GameObject door8;
    private Animator Door1Anim;
    private Animator Door2Anim;
    private Animator Door3Anim;
    private Animator Door4Anim;
    private Animator Door5Anim; 
    private Animator Door6Anim;
    private Animator Door7Anim;
    private Animator Door8Anim;            
    private bool Opened = false;
    void Start()
    {
        Door1Anim = door1.GetComponent<Animator>();
        Door2Anim = door2.GetComponent<Animator>();
        Door3Anim = door3.GetComponent<Animator>();
        Door4Anim = door4.GetComponent<Animator>();
        Door5Anim = door5.GetComponent<Animator>();
        Door6Anim = door6.GetComponent<Animator>();
        Door7Anim = door7.GetComponent<Animator>();
        Door8Anim = door8.GetComponent<Animator>();
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
        if(Opened==false){
            if(other.gameObject.tag=="Player"){
                Invoke("Opendoor",4.0f);
                Invoke("RemoveObstacle",5.0f);
                Opened = true;
            }
        }
    }
    void Opendoor(){
        Door1Anim.SetTrigger("Open");
        Door2Anim.SetTrigger("Open");
        Door3Anim.SetTrigger("Open");
        Door4Anim.SetTrigger("Open");
        Door5Anim.SetTrigger("Open");
        Door6Anim.SetTrigger("Open");
        Door7Anim.SetTrigger("Open");
        Door8Anim.SetTrigger("Open");
    }
    void RemoveObstacle(){
        Obstacle1.SetActive(false);
        Obstacle2.SetActive(false);
        Obstacle3.SetActive(false);
        Obstacle4.SetActive(false);
        Obstacle5.SetActive(false);
        Obstacle6.SetActive(false);
        Obstacle7.SetActive(false);
        Obstacle8.SetActive(false);
    }
}
