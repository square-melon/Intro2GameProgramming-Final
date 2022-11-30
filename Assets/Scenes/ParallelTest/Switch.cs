using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            if(!inTransition) {
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
        ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, 51, ppos.z);
        DataManager.Instance.SetPlayerPos(new Vector3(ppos.x, 51, ppos.z));
        foreach (GameObject enemy in enemies)
        {
            epos = enemy.transform.position;
            enemy.transform.position = new Vector3(epos.x, 51, epos.z);
        }
    }

    IEnumerator ToOrigin() {
        canvasAnimator.SetTrigger("PSwitch");
        yield return new WaitForSeconds(1);
        ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, 1, ppos.z);
        DataManager.Instance.SetPlayerPos(new Vector3(ppos.x, 1, ppos.z));
        foreach (GameObject enemy in enemies)
        {
            epos = enemy.transform.position;
            enemy.transform.position = new Vector3(epos.x, 1, epos.z);
        }
    }

    IEnumerator Transition() {
        canvasAnimator.SetTrigger("Switch");
        yield return new WaitForSeconds(1);
    }
}
