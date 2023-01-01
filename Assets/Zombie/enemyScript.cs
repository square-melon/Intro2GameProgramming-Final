using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class enemyScript : MonoBehaviour
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
    private int  hp=2;
    private bool first=true;
    public GameObject damagetext;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 3.0f;
    void Start()
    {
        audioPlayer.volume = 5.0f;
        //GameController = GameControllerObj.GetComponent<GameController>();
        naviAgent = this.GetComponent<NavMeshAgent>();
        ZombieAnim = GetComponent<Animator>();
        DataManager.Instance.SetSceneState(false);
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        
        if(dstToPlayer<10.0f && dstToPlayer > 2.0f){ //Track
            if(first){ 
                audioPlayer.PlayOneShot(ZombieMoan);
                first = false;
            }
            transform.LookAt(DataManager.Instance.PlayerPos);
            ZombieAnim.SetFloat("Speed", 1.0f);
            naviAgent.SetDestination(DataManager.Instance.PlayerPos);
        }else if(dstToPlayer<=2.0f){                //attack
            transform.LookAt(DataManager.Instance.PlayerPos);
            ZombieAnim.SetFloat("Speed", 0.0f);
            ZombieAnim.SetBool("Attack",true);
            Invoke("ResetAnimAttack",1.0f);
        } else if(dstToPlayer > 10.0f){
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
        ZombieAnim.SetBool("Attack",false);
    }
    public void Damage() {
        hp--;
    }   
    public void DamagePlayer() {
        DataManager.Instance.PlayerOnHit(1.0f); 
        print(DataManager.Instance._HP);
    }
    public void AttackSE(){
        audioPlayer.PlayOneShot(ZombieAttack);
    }
    public void DeadSE(){
        audioPlayer.PlayOneShot(ZombieDead);
    }
    public void LoadScene2() {

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
