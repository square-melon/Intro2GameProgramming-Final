using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBar : MonoBehaviour
{

    [Header("References")]
    public Transform cam;
    public Transform player;

    [Header("Settings")]
    public float upper = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + new Vector3(0, upper, 0);
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
