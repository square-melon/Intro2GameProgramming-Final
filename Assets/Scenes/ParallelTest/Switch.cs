using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Animator canvasAnimator;

    public GameObject player;
    private GameObject[] enemies;

    // player position
    private Vector3 ppos;
    // enemies position
    private Vector3 epos;

    // TRUE: go to parallel world
    private bool toParallel = false;
    // TRUE: in switching process, we can't do another transition
    private bool inTransition = false;

    private GameObject People;
    private GameObject Monster;
    private NavMeshAgent E_naviAgent;
    private NavMeshAgent P_naviAgent;
    private NavMeshAgent M_naviAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // print(player.transform.position);
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            if(!inTransition && !DataManager.Instance.InBearMode) {
                // inTransition = true;
                toParallel = !toParallel;
                FindAllEnemy();
                if(toParallel) {
                    StartCoroutine(ToParallel());
                } else {
                    StartCoroutine(ToOrigin());
                }
            }
        }
    }

    void FindAllEnemy() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    IEnumerator ToParallel() {
        canvasAnimator.SetTrigger("Switch");
        yield return new WaitForSeconds(1);
        DataManager.Instance.ToggleInParallel();
        foreach (GameObject enemy in enemies)
        {
            People = enemy.transform.GetChild(0).gameObject;
            Monster = enemy.transform.GetChild(1).gameObject;

            epos = Monster.transform.position;

            P_naviAgent = People.GetComponent<NavMeshAgent>();
            P_naviAgent.Warp(new Vector3(epos.x, 51, epos.z));

            M_naviAgent = Monster.GetComponent<NavMeshAgent>();
            M_naviAgent.Warp(new Vector3(epos.x, 51, epos.z));

            People.SetActive(true);
            Monster.SetActive(false);
        }
    }

    IEnumerator ToOrigin() {
        canvasAnimator.SetTrigger("PSwitch");
        yield return new WaitForSeconds(1);
        DataManager.Instance.ToggleInParallel();
        foreach (GameObject enemy in enemies)
        {
            People = enemy.transform.GetChild(0).gameObject;
            Monster = enemy.transform.GetChild(1).gameObject;

            epos = People.transform.position;

            P_naviAgent = People.GetComponent<NavMeshAgent>();
            P_naviAgent.Warp(new Vector3(epos.x, 1, epos.z));

            M_naviAgent = Monster.GetComponent<NavMeshAgent>();
            M_naviAgent.Warp(new Vector3(epos.x, 1, epos.z));

            People.SetActive(false);
            Monster.SetActive(true);
        }
    }

    IEnumerator Transition() {
        canvasAnimator.SetTrigger("Switch");
        yield return new WaitForSeconds(1);
    }
}
