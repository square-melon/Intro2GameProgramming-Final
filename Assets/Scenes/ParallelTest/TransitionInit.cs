using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionInit : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform up;
    public RectTransform down;

    private float correctHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        correctHeight = canvas.GetComponent<RectTransform>().rect.height / 2;

        up.anchoredPosition = new Vector2(0, correctHeight / 2);
        down.anchoredPosition = new Vector2(0, correctHeight / (-2));
        up.sizeDelta = new Vector2(up.sizeDelta.x, canvas.GetComponent<RectTransform>().rect.height / 2);
        down.sizeDelta = new Vector2(down.sizeDelta.x, canvas.GetComponent<RectTransform>().rect.height / 2);
    }
}
