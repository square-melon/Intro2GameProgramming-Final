using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;
using GraphSpace;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TalkManager : MonoBehaviour
{
    // public GameObject Melon;
    // public GameObject Hephaestus;

    public List<GameObject> CharacterList;
    public Animator canvasAnimator;
    public VideoPlayer videoPlayer;

    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("Fight", (x) => { StartCoroutine(Fight(x)); });
    }

    void Clear()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }

    IEnumerator Fight(List<EventValueClass> Value)
    {
        Clear();
        canvasAnimator.SetTrigger("Switch");
        videoPlayer.Play();
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene("03 - Island");
    }
}
