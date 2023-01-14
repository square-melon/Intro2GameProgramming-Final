using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Mentor;

    public GameObject Talk;

    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(Player.transform.position, Mentor.transform.position);
        if(dis <= 10.0f) Talk.SetActive(true);
    }
}
