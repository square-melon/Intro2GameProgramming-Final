using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool PlayerHasKey = false;
    public float Speed;
    private Vector3 RotateAngle;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"){
            PlayerHasKey = true;
        }
    }
    void Start()
    {
        RotateAngle = new Vector3(0,1,0);
    }
    // void Update()
    // {
    //     transform.Rotate(RotateAngle * )
    // }
}
