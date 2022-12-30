using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapDemo : MonoBehaviour {

    //This script goes on the SpikeTrap prefab;

    public Animator spikeTrapAnim; //Animator for the SpikeTrap;
    public float radius;

    public GameObject Player;
    private UnityEngine.AI.NavMeshAgent playerNaviAgent;
    public float originSpeed;

    // Use this for initialization
    void Awake()
    {
        //get the Animator component from the trap;
        spikeTrapAnim = GetComponent<Animator>();
        //start opening and closing the trap for demo purposes;
        StartCoroutine(OpenCloseTrap());
    }
    void Start(){
        Player = GameObject.Find("Player/Human");
        playerNaviAgent = Player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        originSpeed = playerNaviAgent.speed;

    }
    private bool opened = false;
    public static bool trapped = false;
    void Update(){
        if(opened){
            RaycastHit[] hit = Physics.SphereCastAll(transform.position,radius,transform.forward,0);
            foreach(var obj in hit){
                if(obj.collider.CompareTag("Player")){
                    playerNaviAgent.speed = 0f;
                    trapped = true;
                }
            }
        }else{
            playerNaviAgent.speed = originSpeed;
            trapped = false;
        }
    }

    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        spikeTrapAnim.SetTrigger("open");
        opened = true;
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //play close animation;
        spikeTrapAnim.SetTrigger("close");
        opened = false;
        
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }
}
