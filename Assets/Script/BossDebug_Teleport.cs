using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDebug_Teleport : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator ab123;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, DataManager.Instance.PlayerPos) < 5.0f)
            ab123.SetTrigger("FadeOut");
    }
}
