using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;

    [Header("Settings")]
    public float PlayerInitHP;
    public float PlayerBulletDamage;

    private GameObject _PlayerBulletHitObj;
    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = Player.GetComponent<PlayerControl>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init() {

    }

    public void PlayerOnHit(float damage) {
        playerControl.OnHit(damage);
    }

    public void SetPlayerHP(float hp) {
        playerControl.SetPlayerHP(hp);
    }

    public float PlayerHP() {
        return playerControl.GetHP();
    }

    public Vector3 PlayerPos() {
        return playerControl.PlayerPos();
    }

    public void PlayerBulletHitOn(GameObject obj) {
        _PlayerBulletHitObj = obj;
    }
}
