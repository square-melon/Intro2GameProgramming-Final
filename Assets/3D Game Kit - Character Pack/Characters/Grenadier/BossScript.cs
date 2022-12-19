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
    private GameObject FrostRainPrefab;
    public float RotationSlerp;
    private Animator BossAnim;

    [Header("Settings")]
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private int  hp=2;
    private int skill=1;

    [Header("Player")]
    public GameObject Player;
    private UnityEngine.AI.NavMeshAgent playerNaviAgent;
    private Vector3 FacingTarget;
    public Transform shootspot;
    private Coroutine ResetCasting;
    void Start()
    {
        naviAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerNaviAgent = Player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        BossAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Dot(transform.forward, (DataManager.Instance.PlayerPos - transform.position).normalized));
        
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        if (!casting) {
            FaceTarget(DataManager.Instance.PlayerPos - transform.position);
        }
        if(dstToPlayer<25.0f && dstToPlayer > 11.0f){ //Track
            if(!casting){
                Track(DataManager.Instance.PlayerPos);
            }
        }else if(dstToPlayer<=11.0f){                //attack
            if (!naviAgent.isStopped) {
                naviAgent.ResetPath();
                naviAgent.isStopped = true;
            }
            BossAnim.SetFloat("Speed", 0.0f);
            // Debug.Log(Vector3.Dot(transform.forward, (DataManager.Instance.PlayerPos - transform.position).normalized));
            if (Vector3.Dot(transform.forward, (DataManager.Instance.PlayerPos - transform.position).normalized) >= 0.998)
                Attack();
        }
    }
    private bool skill1CD;
    private bool skill2CD;
    private bool casting;
    
    void Attack(){
        BossAnim.SetFloat("Speed", 0.0f);
        if(skill==1 && !skill1CD && !casting){
            BossAnim.SetBool("Attack1",true);
            Invoke("ResetAnimAttack",4.0f);
            skill1CD = true;
            casting = true;
        }else if(skill==2 && !skill2CD && !casting){
            if(playerNaviAgent.velocity.magnitude != 0){
                BossPrediction();
            }
            BossAnim.SetBool("Attack2",true);
            Invoke("ResetAnimAttack2",4.0f);
            skill2CD = true;
            casting = true;
        }
    }
    void BossPrediction(){
        Vector3 Target = new Vector3(1, 0, 0);
        Vector3 a = transform.forward;
        Vector3 b = DataManager.Instance.PlayerPos - transform.position;
        a *= b.magnitude;
        Vector3 b_a = (b - a) * 10f + b;
        transform.rotation = Quaternion.LookRotation(b_a);
    }
    void Spray(){
        // Debug.Log(transform.forward);
        // Debug.Log(transform.forward);
        Vector3 pos = shootspot.position; 
        // pos += Target*0.2;
        ArcaneSprayPrefab = Instantiate(ArcaneSpray, pos, Quaternion.LookRotation(transform.forward));
        Destroy(ArcaneSprayPrefab,3.0f);
    }
    void Rain(){
        Vector3 Target = new Vector3(0,1,0);
        Vector3 pos = DataManager.Instance.PlayerPos; 
        FrostRainPrefab = Instantiate(FrostRain, pos,Quaternion.LookRotation(Target));
        Destroy(FrostRainPrefab,4.0f);
    }
    void Track(Vector3 Target){
        naviAgent.speed = 1.0f;
        BossAnim.SetFloat("Speed", 1.0f);
        naviAgent.SetDestination(Target);
    }
    void ResetAnimAttack(){
        BossAnim.SetBool("Attack1",false);
        casting = false;
        skill1CD = false;
        skill=2;
        naviAgent.isStopped = false;
    }

    // void ResetAttack(){
    //     BossAnim.SetBool("Attack1",false);
    //     casting = false;
    //     skill1CD = false;
    //     skill=2;
    //     naviAgent.isStopped = false;
    // }

    void ResetAnimAttack2(){
        BossAnim.SetBool("Attack2",false);
        casting = false;
        skill2CD = false;
        skill=1;
        naviAgent.isStopped = false;
    }
    void FaceTarget(Vector3 FacingTarget){
        transform.eulerAngles = new Vector3
        (0,Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(FacingTarget), Time.deltaTime * RotationSlerp * 2).eulerAngles.y,0);
    }
}
