using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    // void Awake()
    // {
    //     GameObject[] objs = GameObject.FindGameObjectsWithTag("Score");

    //     if (objs.Length > 1)
    //     {
            
    //         Destroy(this.gameObject);
    //     }

    //     DontDestroyOnLoad(this.gameObject);
    // }
    private TextMeshProUGUI MyScoreText;
    //private int score = 0;
    void Start()
    {
        MyScoreText = FindObjectOfType<TextMeshProUGUI>();
        //print(MyScoreText);
        MyScoreText.SetText("Score: " + ScoreManager.Instance.Score);
    }

    // Update is called once per frame
    void Update()
    {
        MyScoreText = FindObjectOfType<TextMeshProUGUI>();
        //print(MyScoreText);
        MyScoreText.SetText("Score: " + ScoreManager.Instance.Score);
    }
}

