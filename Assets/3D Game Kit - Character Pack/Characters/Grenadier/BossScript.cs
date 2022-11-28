using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("effect")]
    public GameObject ArcaneSpray;
    private GameObject ArcaneSprayPrefab;
    public GameObject FrostRain;

    [Header("Settings")]
    //public float RotationSlerp;
    private Animator BossAnim;
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private int  hp=2;
    private int skill=1;

    private Vector3 FacingTarget;
    public GameObject player;
    void Start()
    {
        naviAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        BossAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        
        if(dstToPlayer<25.0f && dstToPlayer > 11.0f){ //Track
            if(BossAnim.GetCurrentAnimatorStateInfo(0).IsName("GrenadierIdle")){
                Track(DataManager.Instance.PlayerPos);
            }
        }else if(dstToPlayer<=11.0f){                //attack
            naviAgent.speed = 0.0f;
            transform.LookAt(player.transform);  
            BossAnim.SetFloat("Speed", 0.0f);
            Attack();
        }
    }

    
    void Attack(){
        transform.LookAt(player.transform);
            BossAnim.SetFloat("Speed", 0.0f);
            if(skill==1){
                BossAnim.SetBool("Attack1",true);
                Invoke("ResetAnimAttack",1.0f);
            }else if(skill==2){
                BossAnim.SetBool("Attack2",true);
                Invoke("ResetAnimAttack2",1.0f);
                
            }
    }
    private float ArcaneSprayCD;
    private bool SprayCD;
    void Spray(){
        Vector3 Target = DataManager.Instance.PlayerPos - transform.position;
        Vector3 pos = transform.position;
        pos.y = transform.position.y + 0.5f;
        pos.z = transform.position.z - 2.0f;
        ArcaneSprayPrefab = Instantiate(ArcaneSpray, pos, Quaternion.LookRotation(Target));
        Destroy(ArcaneSprayPrefab,2);
    }

    void Track(Vector3 Target){
        naviAgent.speed = 1.0f;
        transform.LookAt(player.transform);  
        BossAnim.SetFloat("Speed", 1.0f);
        naviAgent.SetDestination(Target);
    }
    void ResetAnimAttack(){
        BossAnim.SetBool("Attack1",false);
        skill=2;
    }
    void ResetAnimAttack2(){
        BossAnim.SetBool("Attack2",false);
        skill=1;
    }
    // void FaceTarget(){
    //     transform.eulerAngles = new Vector3
    //     (0, 
    //     Quaternion.Slerp
    //     (transform.rotation,
    //     Quaternion.LookRotation(FacingTarget), 
    //     Time.deltaTime * RotationSlerp * 2).eulerAngles.y,
    //     0);
    // }
    
    
}
