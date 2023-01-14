using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;
using GraphSpace;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TalkManager1 : MonoBehaviour
{
    public List<GameObject> CharacterList;
    public GameObject Talk;

    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("Close", Close);
    }

    void Clear()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }

    void Close(List<EventValueClass> Value)
    {
        Clear();
        Talk.SetActive(false);
    }
}
