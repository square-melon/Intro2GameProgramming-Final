using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class peopleScript : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private Animator peopleAnim;
    Vector3 startSpot;

    public GameObject ExclamationR;
    private GameObject ExclamationPrefabR;
    public GameObject ExclamationY;
    private GameObject ExclamationPrefabY;
    private bool discoveredY = false;
    private bool discoveredR = false;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public AudioSource audiosource;
    public AudioClip hmmclip;
    public AudioClip bclip;
    
    
    //Time
    public static float deltaTime;


    void Start()
    {
        naviAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        // playerNaviAgent = Player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        peopleAnim = GetComponent<Animator>();
        startSpot = transform.position;
    }

    // Update is called once per frame
    private float timer=0;
    bool run = false;
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        if(run==true){
            Run();
        }
        //觸發追逐的條件
        else if(dstToPlayer<10.0f && IsInFace()){
            timer += Time.deltaTime;
            if(timer>=2.0f){
                run = true;
            }
            if(discoveredR==false){
                Destroy(ExclamationPrefabY,0.0f);
                discoveredY = false;
                InstantiateR();     
            }
            Track();
        }
        else if(dstToPlayer<10.0f && IsInFront()){
            if(discoveredY==false){
                audiosource.PlayOneShot(hmmclip);
                Destroy(ExclamationPrefabR,0.0f);
                discoveredR = false;
                InstantiateY();
            } 
            Track();   
            timer=0;
        }
        else if(dstToPlayer<10.0f && SpikeTrapDemo.trapped==true){
            if(discoveredR==false){
                Destroy(ExclamationPrefabY,0.0f);
                discoveredY = false;
                InstantiateR();     
            }
            Track();
        }
        else{
            //idle();
            Patroling();
            timer=0;
        }
        //Timer
        // Debug.Log(timer);
    }
    void TimeCount(){
        
    }

    void idle(){
        naviAgent.SetDestination(startSpot);
        Destroy(ExclamationPrefabY,0.2f);
        Destroy(ExclamationPrefabR,0.2f);
        discoveredY = false;
        discoveredR = false;
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            naviAgent.SetDestination(walkPoint);

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
    void Track(){
        naviAgent.SetDestination(DataManager.Instance.PlayerPos);
    }
    void Run(){
        // Vector3 target = new Vector3(9.867254f,-8.000162f,42.30503f);
        // peopleAnim.SetBool("scared",true);
        // naviAgent.SetDestination(target);
        SpikeTrapDemo.trapped = true;
        audiosource.PlayOneShot(bclip);
        Invoke("RunSpeed",1.0f);
        Invoke("loadScene",2.0f);
    }
    void RunSpeed(){
        naviAgent.speed = 5.0f;
    }
    void loadScene(){
        SceneManager.LoadScene("Lose");
    }
    void InstantiateR(){
        Vector3 pos = transform.position;
        pos.y += 3.0f;
        ExclamationPrefabR = Instantiate(ExclamationR,pos,Quaternion.identity);
        ExclamationPrefabR.transform.parent = transform;
        discoveredR = true;
    }
    void InstantiateY(){
        Vector3 pos = transform.position;
        pos.y += 3.0f;
        ExclamationPrefabY = Instantiate(ExclamationY,pos,Quaternion.identity);
        ExclamationPrefabY.transform.parent = transform;
        discoveredY = true;
    }
    bool IsInFront(){
        Vector3 a = transform.forward;
        Vector3 b = DataManager.Instance.PlayerPos - transform.position;
        if(Vector3.Dot(a,b.normalized)>=0.7){
            return true;     
        }else{
            return false;
        }
    }
    bool IsInFace(){
        Vector3 a = transform.forward;
        Vector3 b = DataManager.Instance.PlayerPos - transform.position;
        if(Vector3.Dot(a,b.normalized)>=0.99){
            return true;     
        }else{
            return false;
        }
    } 
} 
