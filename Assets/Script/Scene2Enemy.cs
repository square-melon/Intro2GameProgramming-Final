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
    public AudioSource audioPlayer;
    public AudioSource deadPlayer;
    public AudioClip attackSE;
    public AudioClip deadSE;

    void Start()
    {
        m_naviAgent = this.enemy.GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {

        DetectDead();
        
        Face = DataManager.Instance.PlayerPos - transform.position;
        dis = Vector3.Distance(DataManager.Instance.PlayerPos, transform.position);
        
        if(dis < 12.0f) {
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
        if (hp <= 0){ 
            animator.SetTrigger("Death");
        }
            
    }
    public void CreateArrow() {
        Face = DataManager.Instance.PlayerPos - enemy.transform.position;
        //Face = new Vector3(Face.x, 0f, Face.z).normalized;
        //Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);

        ArrowPrefab = Instantiate(Arrow, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), Quaternion.LookRotation(Face) * Quaternion.Euler(90, 0, 0));
        
        ArrowPrefab.GetComponent<Rigidbody>().AddForce(Face * 200.0f);
        audioPlayer.PlayOneShot(attackSE);
       
    }
    public void DeadSE(){
        deadPlayer.PlayOneShot(deadSE);
    }
}
