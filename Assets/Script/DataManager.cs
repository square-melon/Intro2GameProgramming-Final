using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public float Score { get; private set; }
    public float _HP { get; private set; }
    public float MAXHP { get; private set; }
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

    public float HP() {
        return _HP;
    }

    public void SetPlayerHP(float hp) {
        _HP = hp;
    }

    public void PlayerOnHit(float damage) {
        _HP -= damage;
    }

    public void HealPlayer(float hp) {
        if (_HP + hp > MAXHP)
            _HP = MAXHP;
        else
            _HP += hp;
    }

    public void SetMAXHP(float hp) {
        MAXHP = hp;
    }
}