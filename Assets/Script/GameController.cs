using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;

    [Header("Settings")]
    public float PlayerMaxHP;
    public float PlayerBulletDamage;

    private GameObject _PlayerBulletHitObj;
    private PlayerControl playerControl;

    void Start()
    {
        playerControl = Player.GetComponent<PlayerControl>();
        Init();
    }

    void Update()
    {
        
    }

    private void Init() {

    }

    public Vector3 PlayerPos() {
        return playerControl.PlayerPos();
    }

    public GameObject PlayerBulletHitOn() {
        return _PlayerBulletHitObj;
    }

    public void _PlayerBulletHitOn(GameObject obj) {
        _PlayerBulletHitObj = obj;
    }
    
    public float CurDashCD() {
        return playerControl.DashCD();
    }

    public float DashCD() {
        return playerControl.DashCooldown;
    }
}
