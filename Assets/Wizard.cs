using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject player;
    public GameObject Fire;

    public float hitRange = 5f;
    public float speed = 5f;

    private Animator animator;
    // private Rigidbody rb;

    private int IdleState;
    private int AttackStartState;
    private int AttackDashState;
    private int GetHitState;
    private int DizzyState;
    private int DieState;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        // rb = gameObject.GetComponent<Rigidbody>();

        IdleState = Animator.StringToHash("Base Layer.Idle03");
        AttackStartState = Animator.StringToHash("Base Layer.Attack02Start");
        AttackDashState = Animator.StringToHash("Base Layer.Attack02Maintain");
        GetHitState = Animator.StringToHash("Base Layer.GetHit");
        DizzyState = Animator.StringToHash("Base Layer.Dizzy");
        DieState = Animator.StringToHash("Base Layer.Die");

        // Fire.transform.position = transform.position;
        Fire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        
        if(state.fullPathHash == IdleState)
        {
            direction = player.transform.position;
            direction.y = 0;

            transform.LookAt(player.transform);
            animator.SetBool("Attack", true);
        }
        else if(state.fullPathHash == AttackStartState)
        {
            Fire.SetActive(true);
        }
        else if(state.fullPathHash == AttackDashState)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        }
        else if(state.fullPathHash == GetHitState)
        {
            Fire.SetActive(false);
            animator.SetBool("HitWall", false);
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.name == "Player")
    //     {
    //         print("Hit");
    //         // playerControl.PlayerOnHit(40);
    //     }
    //     else if(collision.gameObject.layer == 6)
    //     {
    //         animator.SetBool("HitWall", true);
    //         animator.SetBool("Attack", false);
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            print("Hit");
            // playerControl.PlayerOnHit(40);
        }
        if(other.gameObject.layer == 6)
        {
            animator.SetBool("HitWall", true);
            animator.SetBool("Attack", false);
        }
    }
}
