using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Scene2Boss : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    //public 
    //public GameObject ExplodeEffect;
    public GameObject boss;
    public GameObject player;
    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private GameObject ArrowPrefab;
    public GameObject Arrow;
    private float dis;
    private Vector3 Face;
    private float hp = 10;

    void Start()
    {
        m_naviAgent = this.boss.GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {

        DetectDead();
        
        Face = player.transform.position - boss.transform.position;
        dis = Vector3.Distance(player.transform.position, boss.transform.position);
        
        if(dis < 20.0f) {
            Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);
            transform.rotation = rotation;
            
            //Arrow.transform.rotation = rotation;
            animator.SetBool("BossAttack",true);
        }
        // the second argument, upwards, defaults to Vector3.up
        else {
            animator.SetBool("BossAttack",false);
        }
        
        

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Disappear")) {
                boss.SetActive(false);
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
            animator.SetTrigger("BossDeath");
    }
    // public void CreateArrow() {
    //     Face = player.transform.position - boss.transform.position;
    //     Face = new Vector3(Face.x, 0f, Face.z).normalized;
    //     //Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);

    //     ArrowPrefab = Instantiate(Arrow, boss.transform.position, Quaternion.LookRotation(Face) * Quaternion.Euler(90, 0, 0));
        
    //     ArrowPrefab.GetComponent<Rigidbody>().AddForce(Face * 300.0f);
         
       
    // }
    public void CreateBig() {
        Face = player.transform.position - new Vector3(boss.transform.position.x, boss.transform.position.y + 3, boss.transform.position.z);
        
        //Face = new Vector3(Face.x, 0, Face.z).normalized;
        //Quaternion rotation = Quaternion.LookRotation(Face, Vector3.up);

        ArrowPrefab = Instantiate(Arrow, new Vector3(boss.transform.position.x, boss.transform.position.y + 3, boss.transform.position.z), Quaternion.LookRotation(Face) * Quaternion.Euler(90, 0, 0));
        
        ArrowPrefab.GetComponent<Rigidbody>().AddForce(Face * 100.0f);
    }
    public void LoadtoNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
