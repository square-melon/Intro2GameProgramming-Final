using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    int flag = 0;
    public Animator animator;
    public Image image;
    void Start()
    {
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPlay()
    {
        SceneManager.LoadScene("Scene1");
        animator.SetTrigger("FadeOut");
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void RuleGame() 
    {
        if(flag == 0) {
            image.enabled = true;

            flag = 1;
        }
        else {

            image.enabled = false;

            flag = 0;
        }
    }
}
