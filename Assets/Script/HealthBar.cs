using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform player;

    [Header("Settings")]
    public float upper = 0.5f;

    private Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
    }

    void Update() {
        slider.maxValue = DataManager.Instance.MAXHP;
        slider.value = DataManager.Instance.HP();
        transform.position = player.position + new Vector3(0, upper, 0);
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
