using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRefill : MonoBehaviour
{

    [Header("Settings")]
    public float scaling;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 PlayerPos = DataManager.Instance.PlayerPos;
        Vector3 Looking = PlayerPos - transform.position;
        Looking.y = 0;
        Vector3 NextMove = Vector3.Cross(Vector3.up, Looking);
        transform.position += NextMove * scaling;
        transform.rotation = Quaternion.LookRotation(PlayerPos - transform.position);
    }
}
