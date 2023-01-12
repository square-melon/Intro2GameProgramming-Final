using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookieControl : MonoBehaviour
{
    public GameObject Melon;
    public GameObject Hephaestus;
    public GameObject Zombie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(Melon.transform.position, Zombie.transform.position) <= 10f) {
            print("pop second canvas");
        }
    }
}
