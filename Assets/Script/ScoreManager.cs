using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public float Score { get; private set; }
    private void Awake() 
    {   
        Instance = this; 
    }
    // void Start() {
    //     Score = 0.0f;
    // }
    public void IncreaseScore(float amount)
    {
        Score += amount;
    }
}