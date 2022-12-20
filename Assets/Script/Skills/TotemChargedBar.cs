using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemChargedBar : MonoBehaviour
{

    [Header("References")]
    public Transform cam;

    private Slider slider;
    private GameObject HealFill;
    private GameObject ChainFill;
    private GameObject Fill;
    private GameObject LightningFill;

    void Start() {
        slider = GetComponent<Slider>();
        cam = GameObject.Find("Main Camera").transform;
        HealFill = gameObject.transform.Find("HealFill").gameObject;
        ChainFill = gameObject.transform.Find("ChainFill").gameObject;
        Fill = gameObject.transform.Find("Fill").gameObject;
        LightningFill = gameObject.transform.Find("LightningFill").gameObject;
    }

    void Update() {
        slider.maxValue = 100;
        slider.value = DataManager.Instance.TotemCharged;
        if (DataManager.Instance.LightningMode) {
            LightningFill.SetActive(true);
            Fill.SetActive(false);
            slider.fillRect = LightningFill.GetComponent<RectTransform>();
        }
        else {
            LightningFill.SetActive(false);
            Fill.SetActive(true);
            slider.fillRect = Fill.GetComponent<RectTransform>();
        }
        HealFill.SetActive(DataManager.Instance.HealingPlaceCreated);
        ChainFill.SetActive(DataManager.Instance.ChainLightning);
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
