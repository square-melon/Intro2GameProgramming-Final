using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ControlGrave : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject door1;

    public GameObject zombie1;
    private Animator animator1;

    public GameObject zombie2;
    private Animator animator2;

    public GameObject zombie3;
    private Animator animator3;

    public GameObject zombie4;
    private Animator animator4;

    public GameObject zombie5;
    private Animator animator5;

    public GameObject zombie6;
    private Animator animator6;
    
    public GameObject dusteffect;
    private GameObject dustprefab1;
    private GameObject dustprefab2;
    private GameObject dustprefab3;
    private GameObject dustprefab4;
    private GameObject dustprefab5;
    private GameObject dustprefab6;

    public AudioSource audiosource;
    public AudioClip aclip;
    public GameObject human;
    private NavMeshAgent humannav;

    public Zombie_wake a1;
    public Zombie_wake a2;
    public Zombie_wake a3;
    public Zombie_wake a4;
    public Zombie_wake a5;
    public Zombie_wake a6;
    void Start()
    {
        DataManager.Instance.ShowBossHP = false;
        animator1 = zombie1.GetComponent<Animator>();
        animator2 = zombie2.GetComponent<Animator>();
        animator3 = zombie3.GetComponent<Animator>();
        animator4 = zombie4.GetComponent<Animator>();
        animator5 = zombie5.GetComponent<Animator>();
        animator6 = zombie6.GetComponent<Animator>();
        door1.SetActive(false);
        humannav = human.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Pass();
    }
    private int flagdetect = 0;

    void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Collider>().CompareTag("Player") && flagdetect == 0) {
            
            dustprefab1 = Instantiate(dusteffect,zombie1.transform.position,Quaternion.identity);
            dustprefab2 = Instantiate(dusteffect,zombie2.transform.position,Quaternion.identity);
            dustprefab3 = Instantiate(dusteffect,zombie3.transform.position,Quaternion.identity);
            dustprefab4 = Instantiate(dusteffect,zombie4.transform.position,Quaternion.identity);
            dustprefab5 = Instantiate(dusteffect,zombie5.transform.position,Quaternion.identity);
            dustprefab6 = Instantiate(dusteffect,zombie6.transform.position,Quaternion.identity);
            
            Invoke("destroydust",0.6f);
            Invoke("really",9.0f);
            //zombie1.SetActive(true); 
            humannav.isStopped = true;
            Invoke("stop",8.0f);
            audiosource.PlayOneShot(aclip);
            door1.SetActive(true);
            DataManager.Instance.EnterGrave = true;


            flagdetect = 1;
        }
    }
    void stop() {
        humannav.isStopped = false;
    }
    void destroydust() {
        
        zombie1.SetActive(true);
        animator1.SetTrigger("wake");
        zombie2.SetActive(true);
        animator2.SetTrigger("wake");
        zombie3.SetActive(true);
        animator3.SetTrigger("wake");
        zombie4.SetActive(true);
        animator4.SetTrigger("wake");
        zombie5.SetActive(true);
        animator5.SetTrigger("wake");
        zombie6.SetActive(true);
        animator6.SetTrigger("wake");
    }
    void really() {
        if(dustprefab1)
            Destroy(dustprefab1);
        if(dustprefab2)
            Destroy(dustprefab2);
        if(dustprefab3)
            Destroy(dustprefab3);
        if(dustprefab4)
            Destroy(dustprefab4);
        if(dustprefab5)
            Destroy(dustprefab5);
        if(dustprefab6)
            Destroy(dustprefab6);
    }
    void Pass() {
        Debug.Log(!zombie1.activeSelf && !zombie2.activeSelf && !zombie3.activeSelf && !zombie4.activeSelf && !zombie5.activeSelf && !zombie6.activeSelf);
        if(!zombie1.activeSelf && !zombie2.activeSelf && !zombie3.activeSelf && !zombie4.activeSelf && !zombie5.activeSelf && !zombie6.activeSelf ) {
            door1.SetActive(false);
        }
    }
}
