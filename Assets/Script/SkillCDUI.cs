using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCDUI : MonoBehaviour
{

    public int iam;
    public Image Fill;
    public Text Timer;
    Transform SkillUI;
    public GameObject Icon;
    public GameObject Disable;

    void Start()
    {
    }

    void Update()
    {
        FindWhichSkill();
        Display();
    }

    void FindWhichSkill() {
        string skname = "";
        bool lightning = false;
        bool rooted = false;
        bool IsLightning = DataManager.Instance.LightningMode;
        bool IsRooted = DataManager.Instance.PlayerIsRooted;
        switch (DataManager.Instance.SkillEvent[iam]) {
            case 0: skname = "Dash"; lightning = false; rooted = false; break;
            case 1: skname = "Frost"; lightning = false; rooted = true; break;
            case 2: skname = "Sparky"; lightning = false; rooted = true; break;
            case 3: skname = "LightningMode"; lightning = true; rooted = true; break;
            case 4: skname = "ExploAll"; lightning = false; rooted = true; break;

            case 101: skname = "ExploR"; lightning = false; break;
            case 102: skname = "ExploG"; lightning = false; break;
            case 103: skname = "ExploB"; lightning = false; break;
            default: skname = ""; break;
        }
        for (var i = Icon.transform.childCount - 1; i >= 0; i--) {
            if (Icon.transform.GetChild(i).name != skname) {
                Icon.transform.GetChild(i).gameObject.SetActive(false);
            } else {
                Icon.transform.GetChild(i).gameObject.SetActive(true);
                if ((!lightning && IsLightning) || (!rooted && IsRooted))
                    Disable.SetActive(true);
                else
                    Disable.SetActive(false);
                SkillUI = Icon.transform.GetChild(i);
            }
        }
    }

    void Display() {
        if (!SkillUI) return;

        float cntCD = 0;
        float CurCD = DataManager.Instance.CurSkillCD[iam];
        float MAXCD = DataManager.Instance.MAXSkillCD[iam];

        if ((Mathf.Floor(CurCD * 10f) / 10f) == 0) {
            cntCD = 0;
        } else if (CurCD < 1) {
            cntCD = (Mathf.Floor(CurCD * 10f) / 10f);
        } else {
            cntCD = (Mathf.Ceil(CurCD));
        }
        Fill.fillAmount = CurCD / MAXCD;

        if (cntCD <= 0) 
            Timer.text = "";
        else
            Timer.text = cntCD.ToString();
    }
}
