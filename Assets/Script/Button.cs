using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    int flag = 0;
    public Animator animator;
    public Image image;
    public AudioSource audioPlayer;
    public AudioClip hover;
    public AudioClip click;

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
        DataManager.Instance.SetMAXHP(200);
        DataManager.Instance.SetPlayerHP(200);
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

    public void OnPointerEnter() {
        audioPlayer.PlayOneShot(hover);
    }

    public void OnPointerClick() {
        audioPlayer.PlayOneShot(click);
    }
}
