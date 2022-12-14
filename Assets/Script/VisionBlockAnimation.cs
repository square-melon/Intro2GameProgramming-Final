using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisionBlockAnimation : MonoBehaviour
{

    [Header("References")]
    public Image Filter;

    [Header("Settings")]
    public float FilterAlpha;
    public float FilterSpeed;

    private bool Changing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FilterAlphaSettings();
    }

    void FilterAlphaSettings() {
        if (DataManager.Instance.IsPausing == 0 && DataManager.Instance.InBearMode) {
            float BioVal = DataManager.Instance.BearTime;
            Color Into;
            if (Filter.color.a < BioVal * FilterAlpha * 0.01f * 0.9f)
                Into = Color.Lerp(Filter.color, new Color(0.1002123f, 0.4528302f, 0.04485582f, BioVal * FilterAlpha * 0.01f), Time.deltaTime * FilterSpeed);
            else
                Into = new Color(0.1002123f, 0.4528302f, 0.04485582f, BioVal * FilterAlpha * 0.01f);
            Filter.color = Into;
        } else {
            Color Into;
            if (Filter.color.a > 0.1f)
                Into = Color.Lerp(Filter.color, new Color(0.1002123f, 0.4528302f, 0.04485582f, 0f), Time.deltaTime * FilterSpeed);
            else
                Into = new Color(0.1002123f, 0.4528302f, 0.04485582f, 0f);
            Filter.color = Into;
        }
    }
}
