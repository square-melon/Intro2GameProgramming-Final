using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisionBlockAnimation : MonoBehaviour
{

    [Header("References")]
    public Image Filter;
    public Image[] Vines;

    [Header("Settings")]
    public float FilterAlpha;
    public float VineSpeed;
    public float BreathSpeed;
    public float BreathSpace;

    private Coroutine ColorChange;
    private bool Changing;

    // Start is called before the first frame update
    void Start()
    {
        Changing = false;
        foreach (Image vine in Vines)
        {
            Color c = vine.color;
            c.a = 0;
            vine.color = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FilterAlphaSettings();
        VinesSettings();
    }

    void FilterAlphaSettings() {
        if (DataManager.Instance.IsPausing == 0) {
            float BioVal = DataManager.Instance.BiolanceValue;
            Color Into = Color.Lerp(Filter.color, new Color(0.1002123f, 0.4528302f, 0.04485582f, BioVal * FilterAlpha * 0.01f), Time.deltaTime);
            Filter.color = Into;
        } else {
            Filter.color = new Color(0.1002123f, 0.4528302f, 0.04485582f, 0f);
        }
    }

    void VinesSettings() {
        if (DataManager.Instance.IsPausing == 0 && DataManager.Instance.BiolanceValue >= 60f) {
            foreach (Image vine in Vines) {
                if (!vine.enabled) {
                    Color c = vine.color;
                    c.a = 1;
                    vine.color = c;
                    Changing = false;
                }
            }
            if (!Changing)
                ColorChange = StartCoroutine(RandomBreath());
        } else {
            if (ColorChange != null)
                StopCoroutine(ColorChange);
            foreach (Image vine in Vines)
            {
                Color c = vine.color;
                c.a = 0;
                vine.color = c;
            }
        }
    }

    IEnumerator RandomBreath() {
        Changing = true;
        float duration = 0;
        Color Into = Random.ColorHSV(123f, 123f, 100, 100, 60, 100);
        while (duration <= BreathSpeed) {
            foreach (var vine in Vines) {
                vine.color = Color.Lerp(vine.color, Into, Time.deltaTime * VineSpeed);
                duration += Time.deltaTime;
                yield return null;
            }
        }
        yield return new WaitForSeconds(BreathSpace);
        Changing = false;
    }
}
