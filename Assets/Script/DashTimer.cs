using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashTimer : MonoBehaviour
{

    [Header("References")]
    public TextMeshProUGUI Timer;
    public Image Fill;

    void Update()
    {
        float MAXDashCD = DataManager.Instance.MAXDashCD;
        float CurDashCD = DataManager.Instance.CurDashCD;
        if ((Mathf.Floor(CurDashCD * 10f) / 10f) == 0) {
            Timer.text = "";
        } else if (CurDashCD < 1) {
            Timer.text = (Mathf.Floor(CurDashCD * 10f) / 10f).ToString();
        } else {
            Timer.text = (Mathf.Ceil(CurDashCD)).ToString();
        }
        Fill.fillAmount = CurDashCD / MAXDashCD;
    }
}
