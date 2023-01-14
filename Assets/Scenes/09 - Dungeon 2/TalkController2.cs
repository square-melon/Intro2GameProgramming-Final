using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkController2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject Mentor;
    public GameObject Talk;

    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        Talk.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(Player.transform.position, Mentor.transform.position);
        print(dis);
        if(dis <= 5.0f) {
            Talk.SetActive(true);
        }
    }
}
