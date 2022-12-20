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
    public float ExistTime;
    public float RayRadius;
    public float RainRadius;
    public float RayLength;
    public float SprayDamage;
    public float RainDamage;
    private bool HitPlayer;
    private bool RHitPlayer;

    [Header("Settings")]
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private float  hp=150A;
    private float skill=1;

    [Header("Player")]
    public GameObject Player;
    public GameObject Bear;
    private UnityEngine.AI.NavMeshAgent playerNaviAgent;
    private UnityEngine.AI.NavMeshAgent bearNaviAgent;
    private Vector3 FacingTarget;
    public Transform shootspot;
    private Coroutine ResetCasting;
    private bool Spraying = false;
    private bool Rainning = false;
    private bool Dead ;
    void Start()
    {
        naviAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerNaviAgent = Player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        bearNaviAgent = Bear.GetComponent<UnityEngine.AI.NavMeshAgent>();
        BossAnim = GetComponent<Animator>();
        HitPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Dot(transform.forward, (DataManager.Instance.PlayerPos - transform.position).normalized));
        if(Dead==false){
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
                if (Vector3.Dot(transform.forward, (DataManager.Instance.PlayerPos - transform.position).normalized) >= 0.998)
                    Attack();
            }
            if(Spraying){
                RaycastHit[] hit = Physics.SphereCastAll(transform.position, RayRadius, transform.forward, RayLength);
                foreach (var obj in hit) {
                    if (obj.collider.CompareTag("Player")){
                        if(HitPlayer==false){
                            HitPlayer=true;
                            DataManager.Instance.PlayerOnHit(SprayDamage);
                            Invoke("ResetHitPlayer",0.2f);
                        }
                    }
                }
            }
            if(Rainning){
                RaycastHit[] hit = Physics.SphereCastAll(FrostRainPrefab.transform.position, RainRadius, transform.forward, 0);
                foreach (var obj in hit) {
                    if (obj.collider.CompareTag("Player")){
                        playerNaviAgent.speed = 1.5f;
                        if(RHitPlayer==false){
                            RHitPlayer=true;
                            DataManager.Instance.PlayerOnHit(RainDamage);
                            Invoke("ResetHitPlayerR",0.4f);
                        }
                    }
                }
            }else{
                playerNaviAgent.speed = 3.0f;
            }
            if(hp<=0 ){
                Dead = true;
                BossAnim.SetBool("Dead", true);
                // Destroy(gameObject,5.0f);
            }
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
            if(DataManager.Instance.InBearMode){
                if(bearNaviAgent.velocity.magnitude != 0){
                    BossPrediction();
                }
            }
            if(DataManager.Instance.InBearMode==false){
                if(playerNaviAgent.velocity.magnitude != 0){
                    BossPrediction();
                }
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
        Spraying = true;
        Vector3 pos = shootspot.position; 
        ArcaneSprayPrefab = Instantiate(ArcaneSpray, pos, Quaternion.LookRotation(transform.forward));
        Invoke("DestroyArcaneSpray",2.0f);
    }
    void DestroyArcaneSpray(){
        Spraying = false;
        Destroy(ArcaneSprayPrefab);
    }
    void ResetHitPlayer(){
        HitPlayer = false;
    }
    void Rain(){
        Rainning = true;
        Vector3 Target = new Vector3(0,1,0);
        Vector3 pos = DataManager.Instance.PlayerPos;
        FrostRainPrefab = Instantiate(FrostRain, pos,Quaternion.LookRotation(Target));
        Invoke("DestroyRain",3.5f);
    }
    void DestroyRain(){
        Rainning = false;
        Destroy(FrostRainPrefab);
    }
    void ResetHitPlayerR(){
        RHitPlayer = false;
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
        skill = 2;
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
    public void Damage(float damage) {
        hp-= damage;
    }
}
