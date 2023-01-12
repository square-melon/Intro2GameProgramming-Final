using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public float IncreaseBioValRate;
    public float IncBioValRateBoss;
    public static DataManager Instance { get; private set; }
    public float Score { get; private set; }
    public float _HP { get; private set; }
    public float MAXHP { get; private set; }
    public bool IsPlayerDead { get; private set; }
    public int PreviousScene { get; private set;}
    public bool SceneWin { get; private set; }
    public Vector3 PlayerPos { get; private set; }
    public Quaternion PlayerRot { get; private set; }
    public Vector3 PlayerFacing { get; private set; }
    public Vector3 PlayerScale { get; private set; }
    public float BiolanceValue { get; private set; }
    public int IsPausing;
    public int[] SkillLevel { get; private set; }
    public int[] SkillEvent { get; private set; }
    public float[] MAXSkillCD { get; private set; }
    public float[] CurSkillCD { get; private set; }
    public bool LightningMode { get; private set; }
    public bool InParallel { get; private set; }
    public bool PlayerIsRooted { get; private set; }
    public float RootedTime { get; private set; }
    public int[] BearSkill { get; private set; }
    public int BearTime;
    public bool InBearMode;
    public float ShieldStored;
    public float ShieldBlockPer;
    public float ShieldBlockDamagePer;
    public float MaxShieldStored;
    public bool ShieldUp;
    public float Scaling;
    public int ShootOnSB;
    public GameObject damagetext;
    public GameObject cam;
    public float TotemCharged;
    public bool HealingPlaceCreated;
    public bool ChainLightning;
    public bool ShakeCam;
    public float stamina;
    public bool EmitGhost;
    public bool PlayerKnockDown;
    public GameObject KnockDownFrom;
    public bool BossStage;
    public bool Burner;
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
            BearSkill = new int[5]{-1, -1, -1, -1, -1};
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
        if (ShieldUp) {
            _HP -= ShieldBlockDamagePer * damage * 0.01f;
            ShieldStored += ShieldBlockPer * damage * 0.01f;
            if (ShieldStored > MaxShieldStored)
                ShieldStored = MaxShieldStored;
            var go = Instantiate(damagetext, PlayerPos, Quaternion.identity);
            go.GetComponent<TextMesh>().text = (ShieldBlockDamagePer * damage * 0.01f).ToString("N1");
            go.transform.LookAt(go.transform.position + cam.transform.forward);
        }
        else {
            _HP -= damage;
            var go = Instantiate(damagetext, PlayerPos, Quaternion.identity);
            go.GetComponent<TextMesh>().text = damage.ToString("N1");
            Debug.Log(_HP);
            // go.transform.LookAt(go.transform.position + cam.transform.forward);
        }
        if (!BossStage)
            BiolanceValue += damage * IncreaseBioValRate * 0.01f;
        else
            BiolanceValue += damage * IncBioValRateBoss * 0.01f;
        if (BiolanceValue >= 100f)
            BiolanceValue = 100f;
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

    public void IsRooted(float rootedTime) {
        PlayerIsRooted = true;
        RootedTime = rootedTime;
    }

    public void ToggleRooted() {
        PlayerIsRooted = !PlayerIsRooted;
    }

    public void takedamage(Transform enemy, float damage) {
        enemy = ChangeToEnemyTrans(enemy);
        Debug.Log(enemy.name);
        if (enemy.CompareTag("Player")) {
            PlayerOnHit(damage);
            return;
        }
        if (!enemy.CompareTag("Enemy")) {
            for (int i = enemy.childCount - 1; i >= 0; i--) {
                if (enemy.GetChild(i).CompareTag("Enemy")) {
                    enemy = enemy.GetChild(i);
                    break;
                }
            }
        }
        ShootOnSB++;
        Scene2Enemy e1 = enemy.GetComponent<Scene2Enemy>();
        enemyScript e2 = enemy.GetComponent<enemyScript>();
        Zombie3script ee2 = enemy.GetComponent<Zombie3script>();
        Wizard e3 = enemy.GetComponent<Wizard>();
        Scene2Boss ee = enemy.GetComponent<Scene2Boss>();
        EnemySpider e4 = enemy.GetComponent<EnemySpider>();
        BossScript e5 = enemy.GetComponent<BossScript>();
<<<<<<< Updated upstream
        SpiderCreate e6 = enemy.GetComponent<SpiderCreate>();
=======
        FireDemon FD = enemy.GetComponent<FireDemon>();
>>>>>>> Stashed changes
        if(e4) {
            e4.DamageA();
            print("joikokij");
        }
        if (e1)
            e1.Damage(damage);
        else if (e2)
            e2.Damage();
        else if (e3)
            e3.Damage(damage);
        else if (ee)
            ee.Damage();
        else if (ee2)
            ee2.Damage();
        else if (e5)
            e5.Damage(damage);
<<<<<<< Updated upstream
        else if (e6)
            e6.Damage(damage);
=======
        else if (FD)
            FD.Damage(damage);

>>>>>>> Stashed changes
        if(damagetext) {
            ShowDamage(enemy, damage);
        }
    }

    public void ShakeCamera() {
        ShakeCam = true;
    }

    void ShowDamage(Transform enemy, float damage) {
        var go = Instantiate(damagetext, enemy.transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = damage.ToString("N1");
        // go.transform.LookAt(go.transform.position + cam.transform.forward);
    }
    private Transform ChangeToEnemyTrans(Transform Enemy) {
        if (Enemy.CompareTag("Enemy")) return Enemy;
        if (Enemy.CompareTag("Player")) return Enemy;
        for (int j = Enemy.childCount - 1; j >= 0; j--) {
            if (Enemy.GetChild(j).gameObject.activeSelf && Enemy.GetChild(j).CompareTag("Enemy")) {
                return Enemy.GetChild(j);
            }
        }
        return null;
    }

    public void IncreaseBioVal(float val) {
        BiolanceValue += val;
    }

    public void SetPlayerRotation(Quaternion rot) {
        PlayerRot = rot;
    }

    public void SetPlayerFacing(Vector3 forw) {
        PlayerFacing = forw;
    }

    public void SetPlayerScale(Vector3 sca) {
        PlayerScale = sca;
    }

    public void BurnPlayer() {
        Burner = true;
    }
}