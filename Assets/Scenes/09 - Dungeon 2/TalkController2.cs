using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;
using GraphSpace;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TalkController2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject Mentor;
    public GameObject Talk;

    public List<GameObject> CharacterList;
    public Animator canvasAnimator;
    public VideoPlayer videoPlayer;

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
        if(dis <= .0f) {
            StartCoroutine(Fight());
        }
    }

    IEnumerator Fight()
    {
        canvasAnimator.SetTrigger("Switch");
        videoPlayer.Play();
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene("03 - Island");
    }
}
