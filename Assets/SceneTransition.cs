using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Wizard W1;
    public Wizard W2;
    public Wizard W3;
    public Wizard W4;
    public Wizard W5;

    public Animator animator;
    // int flag = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(W1.GetHp() <= 0 && W2.GetHp() <= 0 && W3.GetHp() <= 0 && W4.GetHp() <= 0 && W5.GetHp() <= 0) {
        //     animator.SetTrigger("FadeOut");
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        // }
        if(W1.GetHp() <= 0) {
            animator.SetTrigger("FadeOut");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
}
