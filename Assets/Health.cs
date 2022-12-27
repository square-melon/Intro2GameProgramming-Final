using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("References")]
    public Transform cam;
    public GameObject boss;

    [Header("Settings")]
    public float upper = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = boss.transform.position + new Vector3(0, upper, 0);
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
