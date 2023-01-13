using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ControlDoor : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject door1;
    public GameObject door2;
    public Wizard Wizard1;
    public AudioSource audiosource;
    public AudioClip aclip;
    public Animator animator;
    public Text _text;
    private NavMeshAgent agent1;
    private NavMeshAgent agent2;
    public GameObject Human;
    public GameObject Bear;
    public Animator Wizardani;
    public GameObject Wizardd1;
    public GameObject Wizardd2;
    public GameObject Wizardd3;
    // public GameObject Wizardd4;
    // public GameObject Wizardd5;
    public GameObject fireEffect;
    private GameObject fireprefab1;
    private GameObject fireprefab2;
    private GameObject fireprefab3;
    void Start()
    {
        door1.SetActive(false);
        door2.SetActive(false);
        agent1 = Human.GetComponent<NavMeshAgent>();
        agent2 = Bear.GetComponent<NavMeshAgent>();
       
    }

    // Update is called once per frame
    void Update()
    {
        Unlockdoor();
        
    }
    void Unlockdoor() {
        if(Wizard1.GetHp() <= 0) {
            change1();
        }
    }
    void change1() {
        door1.SetActive(false);
        door2.SetActive(false);
    }
    void change2() {
        door1.SetActive(true);
        door2.SetActive(true);
    }
    private int flag = 0;
    private int flagdetect = 0;

    
    void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Collider>().CompareTag("Player") && flagdetect == 0) {
            change2();
            audiosource.PlayOneShot(aclip);
            Vector3 a = new Vector3(Wizardd1.transform.position.x,Wizardd1.transform.position.y-6.0f,Wizardd1.transform.position.z);
            Vector3 b = new Vector3(Wizardd2.transform.position.x,Wizardd2.transform.position.y-6.0f,Wizardd2.transform.position.z);
            Vector3 c = new Vector3(Wizardd3.transform.position.x,Wizardd3.transform.position.y-6.0f,Wizardd3.transform.position.z);
            fireprefab1 = Instantiate(fireEffect,a,Quaternion.identity);
            fireprefab2 = Instantiate(fireEffect,b,Quaternion.identity);
            fireprefab3 = Instantiate(fireEffect,c,Quaternion.identity);
            animator.SetTrigger("red");
            _text.text = "<b>Warning!!!!</b>";
            Invoke("wizardout", 4.5f);
            Invoke("Cleartext", 5.0f);
            if(flag == 0) {
                agent1.isStopped = true;
                //agent2.isStopped = true;
            } 
            flagdetect = 1;
        }
    }
    void wizardout() {
        Wizardd1.SetActive(true);
        Wizardd2.SetActive(true);
        Wizardd3.SetActive(true);
    }
    void Cleartext() {
        _text.text = "";
        flag = 1;
        agent1.isStopped = false;
        //agent2.isStopped = false;
        //Wizardani.SetTrigger("goup");
        Destroy(fireprefab1);
        Destroy(fireprefab2);
        Destroy(fireprefab3);
    }
}
