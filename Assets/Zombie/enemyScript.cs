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
    public GameObject GameControllerObj;
    public GameObject player;
    private GameController GameController;
    private NavMeshAgent naviAgent;
    private Animator ZombieAnim;
    private UnityEngine.AI.NavMeshAgent nav;
    private int  hp=2;
    private bool first=true;

    void Start()
    {
        audioPlayer.volume = 5.0f;
        GameController = GameControllerObj.GetComponent<GameController>();
        naviAgent = this.GetComponent<NavMeshAgent>();
        ZombieAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, GameController.PlayerPos());
        
        if(dstToPlayer<10.0f && dstToPlayer > 1.0f){ //Track
            if(first){
                audioPlayer.PlayOneShot(ZombieMoan);
                first = false;
            }
            transform.LookAt(player.transform);
            ZombieAnim.SetFloat("Speed", 1.0f);
            naviAgent.SetDestination(GameController.PlayerPos());
        }else if(dstToPlayer<=1.0f){                //attack
            transform.LookAt(player.transform);
            ZombieAnim.SetFloat("Speed", 0.0f);
            ZombieAnim.SetBool("Attack",true);
             Invoke("ResetAnimAttack",1.0f);
        }
        if(hp<=0){
            ZombieAnim.SetBool("Dead",true);
        }
        if(ZombieAnim.GetCurrentAnimatorStateInfo(0).IsName("Dissapear")){ //dissapear after dead
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
        DataManager.Instance.PlayerOnHit(50.0f); 
        print(DataManager.Instance._HP);
    }
    public void AttackSE(){
        audioPlayer.PlayOneShot(ZombieAttack);
    }
    public void DeadSE(){
        audioPlayer.PlayOneShot(ZombieDead);
    }
} 
