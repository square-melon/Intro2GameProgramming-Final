using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySpider : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private Transform player;
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
    public float health;
    //States
    public float sightRange = 10.0f;
    public float attackRange = 5.0f;
    public bool playerInSightRange, playerInAttackRange;
    public LayerMask whatground;
    public Animator animator;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    void Start() {
        animator.SetTrigger("Roar");
        audiosource.PlayOneShot(aclip);
    }
    void Death() {
        if(health <= 0.0f) {
            animator.SetTrigger("Die");
        }
    }
    private void Update()
    {
        //Check for sight and attack range
        float dis = Vector3.Distance(player.position,transform.position);
       
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
            print(1);
            animator.SetBool("Move Forward Fast",true);
            //atroling();
        }
        if (playerInSightRange && !playerInAttackRange) {
            print(2);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange) {
            print(3);
            //animator.SetBool("Move Forward Fast",false);
            AttackPlayer();
        }
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
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code
            animator.SetTrigger("Claw Attack");
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
