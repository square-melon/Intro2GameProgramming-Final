using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public GameObject G1;
    public GameObject G2;
    public GameObject TransparentBlock;

    public Animator animator;

    private Scene2Enemy g1;
    private Scene2Enemy g2;
    // Start is called before the first frame update
    void Start()
    {
        g1 = G1.GetComponent<Scene2Enemy>();
        g2 = G2.GetComponent<Scene2Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        print(g1.getHp() + "/" + g2.getHp());
        if(g1.getHp() <= 0.0f && g2.getHp() <= 0.0f) {
            TransparentBlock.SetActive(false);
            animator.SetTrigger("Up");
        }
    }
}
