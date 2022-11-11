using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private NavMeshAgent naviAgent;

    void Start()
    {
        naviAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(dstToPlayer<30.0f){
            naviAgent.SetDestination(player.transform.position);
        }
    }
}
