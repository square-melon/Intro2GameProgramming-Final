using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Transformplace : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private GameObject bear;
    public GameObject point;
    private UnityEngine.AI.NavMeshAgent playerNaviAgent;
    private UnityEngine.AI.NavMeshAgent bearNaviAgent;
    void Start()
    {
        player = GameObject.Find("Human");
        bear = GameObject.Find("Bear");
        playerNaviAgent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        bearNaviAgent = bear.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        float dis = Vector3.Distance(DataManager.Instance.PlayerPos,transform.position);
        print(dis);
        if(dis < 40.0f) {
            playerNaviAgent.SetDestination(point.transform.position);
            bearNaviAgent.SetDestination(point.transform.position);
        }
    }
}
