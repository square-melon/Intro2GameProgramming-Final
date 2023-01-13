using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTextSupport;

public class CharactorManage : MonoBehaviour
{
    public List<GameObject> CharacterList;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().AddEventListener("PlayText.Player.none", Player);
        EventCenter.GetInstance().AddEventListener("PlayText.Mentor.none", Mentor);
    }

    void NextDialogue()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }

    void Player() => CharacterList[0].SetActive(true);

    void Mentor() => CharacterList[1].SetActive(true);
}
