using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitch : MonoBehaviour
{
    public GameObject monster;
    public GameObject people;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToPeople() {
        monster.SetActive(false);
        people.SetActive(true);
    }

    public void ToMonster() {
        monster.SetActive(true);
        people.SetActive(false);
    }
}
