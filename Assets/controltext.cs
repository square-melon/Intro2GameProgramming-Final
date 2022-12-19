using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controltext : MonoBehaviour
{
    // Start is called before the first frame update
    public int destroyrime = 3;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 randomdir = new Vector3(0.5f,0,0);
    void Start()
    {
        Destroy(gameObject, destroyrime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomdir.x, randomdir.x),
        Random.Range(-randomdir.y, randomdir.y),
        Random.Range(-randomdir.z, randomdir.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
