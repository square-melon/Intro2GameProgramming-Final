using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    public int BasedOn;
    public TextMeshProUGUI txt;
    private bool inactive;

    void Start() {
        slider = GetComponent<Slider>();
    }

    void Update() {
        if (BasedOn == 0) {
            slider.maxValue = DataManager.Instance.MAXHP;
            slider.value = DataManager.Instance._HP;
        } else if (BasedOn == 1) {
            slider.maxValue = 100;
            slider.value = DataManager.Instance.BiolanceValue;
        } else if (BasedOn == 2) {
            slider.maxValue = 100;
            slider.value = DataManager.Instance.stamina;
        } else if (BasedOn == 3) {
            if (DataManager.Instance.ShowBossHP) {
                if (!inactive) {
                    inactive = true;
                    for (int j = transform.childCount - 1; j >= 0; j--)
                        transform.GetChild(j).gameObject.SetActive(true);
                }
                txt.text = DataManager.Instance.BossName;
                slider.maxValue = DataManager.Instance.BossMAXHP;
                slider.value = DataManager.Instance.BossHP;
            } else {
                if (inactive) {
                    inactive = false;
                    for (int j = transform.childCount - 1; j >= 0; j--) {
                        Debug.Log(transform.GetChild(j).name);
                        transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
