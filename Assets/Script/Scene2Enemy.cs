using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Scene2Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    //public 
    //public GameObject ExplodeEffect;
    public GameObject enemy;
    public GameObject player;
    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private GameObject ArrowPrefab;
    public GameObject Arrow;
    private float dis;
    private Vector3 Face;
    private float hp = 2;
    void Start()
    {
        m_naviAgent = this.enemy.GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {

        DetectDead();
        
        Face = player.transform.position - enemy.transform.position;
        dis = Vector3.Distance(player.transform.position, enemy.transform.position);
        
        if(dis < 8.0f) {
            Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);
            transform.rotation = rotation;
            
            //Arrow.transform.rotation = rotation;
            animator.SetBool("Attack",true);
        }
        // the second argument, upwards, defaults to Vector3.up
        else {
            animator.SetBool("Attack",false);
        }
        
        

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Disappear")) {
                enemy.SetActive(false);
        }
        
       
        
    }

    // void OnCollisionEnter(Collision col) {
    //     if(col.gameObject.tag == "Bullet") {
    //         //animator.SetTrigger("Death");
    //         hp--;
    //         if(hp <= 0 ){
    //             animator.SetTrigger("Death");
    //         }
    //         print("yes");
    //     }
    // }
    public void Damage() {
        hp--;
        Debug.Log(hp);
    }
    void DetectDead() {
        if (hp <= 0)
            animator.SetTrigger("Death");
    }
    public void CreateArrow() {
        Face = player.transform.position - enemy.transform.position;
        Face = new Vector3(Face.x, 0f, Face.z).normalized;
        //Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);

        ArrowPrefab = Instantiate(Arrow, enemy.transform.position, Quaternion.LookRotation(Face) * Quaternion.Euler(90, 0, 0));
        
        ArrowPrefab.GetComponent<Rigidbody>().AddForce(Face * 300.0f);
         
       
    }

}
