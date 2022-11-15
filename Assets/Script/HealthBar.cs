using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
    }

    void Update() {
        slider.maxValue = DataManager.Instance.MAXHP;
        slider.value = DataManager.Instance.HP();
    }
}
