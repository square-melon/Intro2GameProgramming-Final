using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Wizard : MonoBehaviour
{
    //public GameObject player;
    public GameObject Fire;

    //public AudioSource audioPlayer;
    //public AudioClip attackSE;

    public float hitRange = 5f;
    public float speed = 5f;

    private Animator animator;
    private Transform tf;

    private int IdleState;
    private int AttackStartState;
    private int AttackDashState;
    private int GetHitState;
    private int DizzyState;
    private int DieState;

    private Vector3 direction;

    private float hp = 150;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();

        IdleState = Animator.StringToHash("Base Layer.Idle03");
        AttackStartState = Animator.StringToHash("Base Layer.Attack02Start");
        AttackDashState = Animator.StringToHash("Base Layer.Attack02Maintain");
        GetHitState = Animator.StringToHash("Base Layer.GetHit");
        DizzyState = Animator.StringToHash("Base Layer.Dizzy");
        DieState = Animator.StringToHash("Base Layer.Die");

        // Fire.transform.position = transform.position;
        Fire.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        AttackPlayer();
        Dead();
    }
    void Dead() {
        if(hp == 0) {
            hp = 0;
            animator.SetBool("Dead", true);
            Fire.SetActive(false);
            Invoke("Disappear",1.5f);
        }
    }
    void Disappear() {
        gameObject.SetActive(false);
    }
    public float attackRange;
    private int attackflag = 0;
    Vector3 lockedpoint;
    void AttackPlayer() {
        float dis = Vector3.Distance(DataManager.Instance.PlayerPos, transform.position);
        if(dis < attackRange) {
            if(attackflag == 0) {
                Rush();
            } else {
                agent.SetDestination(lockedpoint);
            }
            Invoke("Rush",5.0f);
            attackflag = 0;
            //Fire.SetActive(false);
        } else {
            agent.SetDestination(transform.position);
        }

        if (dis < 1f)
            attackflag = 0;
    }
    void Rush() {
        attackflag = 1;
        lockedpoint = DataManager.Instance.PlayerPos;
        
        //transform.LookAt(DataManager.Instance.PlayerPos);
        animator.SetBool("Attack", true);
        Fire.SetActive(true);
        agent.speed = 15.0f;
        //agent.SetDestination(lockedpoint);
        
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if(hp > 0) DataManager.Instance.PlayerOnHit(40);
        }
        if(other.gameObject.layer == 6)
        {
            animator.SetBool("HitWall", true);
            animator.SetBool("Attack", false);
        }
    }

    public void Damage(float damage) {
        hp -= damage;
    }

    public float GetHp() {
        return hp;
    }

    // void FixedUpdate()
    // {
    //     AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        
    //     if(state.fullPathHash == IdleState)
    //     {
    //         direction = player.transform.position;
    //         direction.y = 0;

    //         transform.LookAt(player.transform);
    //         animator.SetBool("Attack", true);
    //     }
    //     else if(state.fullPathHash == AttackStartState)
    //     {
    //         Fire.SetActive(true);
    //         // audioPlayer.PlayOneShot(attackSE);
    //     }
    //     else if(state.fullPathHash == AttackDashState)
    //     {
    //         transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
    //     }
    //     else if(state.fullPathHash == GetHitState)
    //     {
    //         Fire.SetActive(false);
    //         animator.SetBool("HitWall", false);
    //     }
    // }
}
