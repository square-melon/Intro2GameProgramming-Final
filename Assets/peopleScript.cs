using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peopleScript : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private Animator peopleAnim;

    public GameObject ExclamationR;
    private GameObject ExclamationPrefabR;
    public GameObject ExclamationY;
    private GameObject ExclamationPrefabY;
    // Start is called before the first frame update
    Vector3 startSpot;
    private bool discoveredY = false;
    private bool discoveredR = false;
    void Start()
    {
        naviAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        // playerNaviAgent = Player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        peopleAnim = GetComponent<Animator>();
        startSpot = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, DataManager.Instance.PlayerPos);
        if(dstToPlayer<15.0f && IsInFace()){
            if(discoveredR==false){
                Destroy(ExclamationPrefabY,0.0f);
                discoveredY = false;
                InstantiateR();
                Track();
            }
        }
        else if(dstToPlayer<15.0f && IsInFront()){
            if(discoveredY==false){
                Destroy(ExclamationPrefabR,0.0f);
                discoveredR = false;
                InstantiateY();
                Track();
            }    
        }
        else{
            idle();
        }
        
    }
    void idle(){
        naviAgent.SetDestination(startSpot);
        Destroy(ExclamationPrefabY,0.2f);
        Destroy(ExclamationPrefabR,0.2f);
        discoveredY = false;
        discoveredR = false;
    }
    
    void Track(){
        naviAgent.SetDestination(DataManager.Instance.PlayerPos);
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
