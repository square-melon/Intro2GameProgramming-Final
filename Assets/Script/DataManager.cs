using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public float IncreaseBioValRate;
    public static DataManager Instance { get; private set; }
    public float Score { get; private set; }
    public float _HP { get; private set; }
    public float MAXHP { get; private set; }
    public bool IsPlayerDead { get; private set; }
    public int PreviousScene { get; private set;}
    public bool SceneWin { get; private set; }
    public Vector3 PlayerPos { get; private set; }
    public float BiolanceValue { get; private set; }
    public int IsPausing;
    public int[] SkillLevel { get; private set; }
    public int[] SkillEvent { get; private set; }
    public float[] MAXSkillCD { get; private set; }
    public float[] CurSkillCD { get; private set; }
    public bool LightningMode { get; private set; }
    public bool InParallel { get; private set; }
    private void Awake()
    {   
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            SkillLevel = new int[10];
            SkillEvent = new int[4]{-1, -1, -1, -1};
            MAXSkillCD = new float[4]{-1, -1, -1, -1};
            CurSkillCD = new float[4];
            DontDestroyOnLoad(this.gameObject);
        }
    }

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
        if (BiolanceValue >= 20f) {
            BiolanceValue += damage * IncreaseBioValRate;
        }
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

    public void PlayerDead(bool state) {
        IsPlayerDead = state;
    }

    public void SetSkillCD(int id, float cd) {
        CurSkillCD[id] = cd;
    }

    public void SetMAXSkillCD(int id, float cd) {
        MAXSkillCD[id] = cd;
    }

    public void SetPreviousScene(int scene) {
        PreviousScene = scene;
    }

    public int GetPreviousScene() {
        return PreviousScene;
    }

    public void SetSceneState(bool state) {
        SceneWin = state;
    }

    public void SetPlayerPos(Vector3 pos) {
        PlayerPos = pos;
    }

    public void SetBiolanceValue(float val) {
        BiolanceValue = val;
    }

    public void IncreaseBiolanceValue(float val) {
        BiolanceValue += val;
    }

    public void SetSkillLevel(int id, int lev) {
        SkillLevel[id] = lev;
    }

    public void SetSkillEvent(int id, int e) {
        SkillEvent[id] = e;
    }

    public void SetLightningMode(bool m) {
        LightningMode = m;
    }

    public void SetInParallel(bool m) {
        InParallel = m;
    }

    public void ToggleLightningMode() {
        LightningMode = !LightningMode;
    }

    public void ToggleInParallel() {
        InParallel = !InParallel;
    }
}