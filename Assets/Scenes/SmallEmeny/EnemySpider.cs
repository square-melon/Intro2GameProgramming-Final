using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySpider : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private Vector3 player;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public AudioSource audiosource;
    public AudioClip aclip;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;
    private float health = 70.0f;
    //States
    public float sightRange = 10.0f;
    public float attackRange = 5.0f;
    public bool playerInSightRange, playerInAttackRange;
    public LayerMask whatground;
    public Animator animator;
    private int dead = 0;
    public GameObject ExplodeEffect;
    private void Awake()
    {
        //player = DataManager.Instance.PlayerPos;
        agent = GetComponent<NavMeshAgent>();
    }
    void Start() {
        //animator.SetTrigger("Roar");
        //audiosource.PlayOneShot(aclip);
    }
    
    private void Update()
    {
        //Check for sight and attack range
        float dis = Vector3.Distance(DataManager.Instance.PlayerPos,transform.position);
        print(dis);
        if(dis < sightRange) {
            playerInSightRange = true;
        } else {
            playerInSightRange = false;
        }
        if(dis < attackRange) {
            playerInAttackRange = true;
        } else {
            playerInAttackRange = false;
        }

        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("Move Forward Fast",true);
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange) {
            print("yes");
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange) {
            //animator.SetBool("Move Forward Fast",false);
            AttackPlayer();
        }
        Death();
    }
    public void DamageA() {
        //print(damage);
        health -= 30;
        Debug.Log(health);
    }
    void Death() {
        if(dead == 0) {
            if(health <= 0.0f) {
                animator.SetTrigger("Die");
                Invoke("ClearSpider",0.7f);
                agent.SetDestination(transform.position);
                dead = 1;
            }
        }
    }
    private void ClearSpider() {
        gameObject.SetActive(false);
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

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

    private void ChasePlayer()
    {
        agent.SetDestination(DataManager.Instance.PlayerPos);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(DataManager.Instance.PlayerPos);

        if (!alreadyAttacked)
        {
            ///Attack code here
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code
            animator.SetTrigger("Claw Attack");
            Instantiate(ExplodeEffect, player, Quaternion.identity);
            DataManager.Instance.PlayerOnHit(30.0f);

            //animator.SetTrigger("Cast Spell");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // public void TakeDamage(int damage)
    // {
    //     health -= damage;

    //     if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    // }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
