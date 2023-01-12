using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    public int BasedOn;

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
        }
    }
}
