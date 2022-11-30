using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SpiderSilk : MonoBehaviour
{
    private GameObject player;
    private GameObject smallspider;
    private RaycastHit hit;
    // Start is called before the first frame update
    // public GameObject smallspider;
    // public GameObject Boss;
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     Vector3 Face = player.transform.position - Boss.transform.position;
    //     Ray ray = new Ray(transform.position,Face);
    //     if (Physics.Raycast(ray, out hit)) {
    //         if (hit.collider.CompareTag("Player")) {
    //             smallspider.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.transform.position);
    //         }
    //     }
    // }
    void Start() {
        player = GameObject.Find("Player");
        smallspider = GameObject.Find("Spider Enemy");
    }
    void Update() {
        ShootSilk();
    }
    public void ShootSilk() {
        Vector3 Face = player.transform.position - transform.position;
        Vector3 position = new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z);
        Vector3 newface = new Vector3(Face.x,Face.y+0.5f,Face.z).normalized;
        
        Ray ray = new Ray(transform.position,newface);
        if (Physics.Raycast(ray, out hit,2f)) {
                if (hit.collider.CompareTag("Player")) {
                    smallspider.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                    smallspider.GetComponent<NavMeshAgent>().speed = 15.0f;
                    //DataManager.Instance.IsRooted(true);
                    Invoke(nameof(NoRooted),1);
                    Destroy(gameObject); 

                    
                }
            } else {
                    
                    //Patroling();
                }
    }
    private void NoRooted() {
        DataManager.Instance.IsRooted(false);
        
    }
}
