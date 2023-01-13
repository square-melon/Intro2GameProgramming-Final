using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Zombie_wake : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioPlayer;
    public AudioClip ZombieAttack;
    public AudioClip ZombieMoan;
    public AudioClip ZombieDead; 
    //public GameObject GameControllerObj;
    //public GameObject player;
    
    private NavMeshAgent naviAgent;
    private Animator ZombieAnim;
    //private UnityEngine.AI.NavMeshAgent nav;
    private float  hp=90;
    private bool first=true;
    public GameObject damagetext;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public GameObject SlashEffect;
    public GameObject SlashEffect1;
    private GameObject zombiehand;
    
    void Start()
    {
        audioPlayer.volume = 5.0f;
        //GameController = GameControllerObj.GetComponent<GameController>();
        naviAgent = this.GetComponent<NavMeshAgent>();
        ZombieAnim = GetComponent<Animator>();
        DataManager.Instance.SetSceneState(false);
        //zombiehand = GameObject.Find("Base HumanRArmDigit12");
        gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    public bool attack = false;
    private bool attacking = false;
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        if(dstToPlayer<10.0f && dstToPlayer > 2.5f){ //Track
            if(first){ 
                audioPlayer.PlayOneShot(ZombieMoan);
                first = false;
            }
            transform.LookAt(DataManager.Instance.PlayerPos);
            ZombieAnim.SetFloat("Speed", 1.0f);
            naviAgent.SetDestination(DataManager.Instance.PlayerPos);
        }else if(dstToPlayer<=2.5f){                //attack
            naviAgent.isStopped = true;
            transform.LookAt(DataManager.Instance.PlayerPos);
            ZombieAnim.SetFloat("Speed", 0.0f);
            if(!attack){
                attack = true;
                ZombieAnim.SetTrigger("Attac");
                Invoke("ResetAnimAttack",1.8f);
            }
            //Vector3 Target = new Vector3(0.0f,0.5f,0.0f);        
        }else if(dstToPlayer > 10.0f){         //patrol
            ZombieAnim.SetFloat("Speed", 0.2f);
            Patroling();
        }

        if(hp<=0){
            ZombieAnim.SetBool("Dead",true);
        }
        if(ZombieAnim.GetCurrentAnimatorStateInfo(0).IsName("Dissapear")){ //dissapear after dead
            gameObject.SetActive(false);
        }
        if(DataManager.Instance.SceneWin == true) {
            gameObject.SetActive(false);
        }
    }
    void ResetAnimAttack(){
        attack = false;
        attacking = false;
    }
    public void Damage(float demage) {
        hp-= demage;
    }   
    public float Gethp() {
        return hp;
    }
    public void DamagePlayer() {
    
        DataManager.Instance.PlayerOnHit(5.0f); 
        print(DataManager.Instance._HP);
    }
    public void AttackSE(){
        audioPlayer.PlayOneShot(ZombieAttack);
        Invoke("Slashins",0.4f);
        Invoke("Slashins1",0.6f);
    }
    public void DeadSE(){
        audioPlayer.PlayOneShot(ZombieDead);
    }
    public void LoadScene2() {

    }
    public void Slashins() {
        SlashEffect.SetActive(true);
        Invoke("Setfalse",1.0f);
        // slashprefab = Instantiate(SlashEffect);
    }
    public void Slashins1() {
        SlashEffect1.SetActive(true);
        Invoke("Setfalse1",1.0f);
    }
    public void Setfalse(){
        SlashEffect.SetActive(false);
    }
    public void Setfalse1(){
        SlashEffect1.SetActive(false);
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            naviAgent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f,whatground))
        // if(walkPoint.x < 30.0f && walkPoint.x > -25.0f && walkpoint)
        walkPointSet = true;
    }
    
} 
