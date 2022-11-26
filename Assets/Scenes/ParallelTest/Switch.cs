using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Animator animator;

    public GameObject player;
    private GameObject[] enemies;

    private Vector3 ppos;
    private Vector3 pos;

    private bool toParallel = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            toParallel = !toParallel;
            FindAllEnemy();
            if(toParallel) {
                StartCoroutine(ToParallel());
                
            } else {
                StartCoroutine(ToOrigin());
                
            }
        }
    }

    void FindAllEnemy() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    IEnumerator ToParallel() {
        animator.SetTrigger("Switch");
        yield return new WaitForSeconds(1);
        ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, 51, ppos.z);
        foreach (GameObject enemy in enemies)
        {
            pos = enemy.transform.position;
            enemy.transform.position = new Vector3(pos.x, 51, pos.z);
        }
    }

    IEnumerator ToOrigin() {
        animator.SetTrigger("PSwitch");
        yield return new WaitForSeconds(1);
        player.transform.position = new Vector3(ppos.x, 1, ppos.z);
        foreach (GameObject enemy in enemies)
        {
            pos = enemy.transform.position;
            enemy.transform.position = new Vector3(pos.x, 1, pos.z);
        }
    }

    IEnumerator Transition() {
        animator.SetTrigger("Switch");
        yield return new WaitForSeconds(1);
    }
}
