using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUni : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject human;
    private GameObject monster;
    private bool flag;
    void Start()
    {
        human = GameObject.Find("Human");
        monster = GameObject.Find("Enemy");
        flag = false;
        human.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("h")) {
            if(flag == false) {
                human.SetActive(true);
                monster.SetActive(false);
                flag = !flag;
            } else {
                human.SetActive(false);
                monster.SetActive(true);
                flag = !flag;
            }
            
        }
    }
}
