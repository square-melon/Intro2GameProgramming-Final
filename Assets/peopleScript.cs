using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peopleScript : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent naviAgent;
    private Animator peopleAnim;

    public GameObject Exclamation;
    private GameObject ExclamationPrefab;  
    // Start is called before the first frame update
    Vector3 startSpot;
    private bool discovered = false;
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
        if(dstToPlayer<15.0f && IsInFront()){
            Track();  
        }else{
            idle();
        }
        
    }
    void idle(){
        naviAgent.SetDestination(startSpot);
        Destroy(ExclamationPrefab,0.2f);
        discovered = false;
    }
    
    void Track(){
        naviAgent.SetDestination(DataManager.Instance.PlayerPos);
        if(discovered==false){
            Vector3 pos = transform.position;
            pos.y += 3.0f;
            ExclamationPrefab = Instantiate(Exclamation,pos,Quaternion.identity);
            ExclamationPrefab.transform.parent = transform;
            discovered = true;
        }
    }
    bool IsInFront(){
        Vector3 a = transform.forward;
        Vector3 b = DataManager.Instance.PlayerPos - transform.position;
        if(Vector3.Dot(a,b.normalized)>=0.9){
            return true;     
        }else{
            return false;
        }
    }
}
