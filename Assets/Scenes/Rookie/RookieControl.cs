using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;
using GraphSpace;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class RookieControl : MonoBehaviour
{
    public GameObject Melon;
    public GameObject Hephaestus;
    public GameObject Zombie;
    public List<GameObject> CharacterList;

    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("MeetMonster", (x) => { StartCoroutine(MeetMonster(x)); });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("KillMonster", (x) => { StartCoroutine(KillMonster(x)); });
        EventCenter.GetInstance().AddEventListener<List<EventValueClass>>("End", End);
    }

    void Clear()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }

    IEnumerator MeetMonster(List<EventValueClass> Value)
    {
        Clear();
        while(true) {
            // print("Melon at " + Melon.transform.position);
            // print("Monster at " + Zombie.transform.position);
            // print(Vector3.Distance(Melon.transform.position, Zombie.transform.position));
            dis = Vector3.Distance(Melon.transform.position, Zombie.transform.position);
            if(dis <= 5.0f) {
                EventNode.FinishThisNode(Value);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator KillMonster(List<EventValueClass> Value)
    {
        Clear();
        while(true) {
            float hp = Zombie.GetComponent<magicZombieScript>().getHp();
            print("Monster's hp is " + hp); 
            if(hp <= 0.0f) {
                EventNode.FinishThisNode(Value);
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    void End(List<EventValueClass> Value)
    {
        SceneManager.LoadScene("Selection");
    }
}
