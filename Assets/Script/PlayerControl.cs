using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Header("Debug")]
    public bool OnDebug = false;

    [Header("References")]
    public Transform rightGunBone;
    public Transform leftGunBone;
    public Transform rightHand;
    public Transform leftHand;
    public Transform ShooterPoint;
    public GameObject rightGun;
    public GameObject LightningMan;
    public GameObject LightningEmt;
    public GameObject ExploBugR;
    public GameObject ExploBugB;
    public GameObject ExploBugG;
    public GameObject Human;
    public GameObject Bear;
    public Transform BearFront;
    public Transform BearLHand;
    public Transform BearRHand;
    public Transform BearShieldUp;
    public GameObject Totem;

    [Header("BearSettings")]
    public Animator PlayerAnim;
    public Animator BearAnim;
    public float BearScale;
    public float HumanScale;
    public float TurnIntoScaler;
    public float TurnIntoDuration;
    public float BearHumanScaleRatio;
    public float HumanNavSpeed;
    public float BearNavSpeed;

    [Header("Bullets")]
    public GameObject Bullet;
    public GameObject FrostBeam;
    public GameObject Sparky;
    public GameObject LightningBullet;
    
    [Header("Effects")]
    public GameObject DashEffect;
    public GameObject FireEffect;
    public GameObject HealEffect;
    public GameObject DamagedEffect;
    public GameObject ChargeEffect;
    public GameObject SparkyChargeEffect;
    public GameObject LightningEffect;
    public GameObject LightningRefill1;
    public GameObject LightningRefill2;
    public GameObject LightningMode;
    public GameObject EarthQuakeEffect;
    public GameObject Shield;
    public GameObject ShieldBreakCircle;
    public GameObject Lightning;

    [Header("Sounds")]
    public AudioSource audioPlayer;
    public AudioClip shootSE;
    public AudioClip walkSE;
    public AudioClip healSE;
    public AudioClip dashSE;
    public AudioClip deadSE;
    public AudioClip hurtSE;
    public AudioClip frostSE;

    [Header("Settings")]
    public float Scaling;
    public Vector3 SpawnPoint;
    public float RotationSlerp;
    public float BearRotationSlerp;
    public float AttackAnimWait;
    public float ReloadSpeed;
    public float ShootForce;
    public float ShootWaitingTime;
    public float Shoot2WaitingTime;
    public float DashCooldown;
    public float DashDistance;
    public float DashSpeed;
    public float MedkitHealHP;
    public float MAXHP;
    public float FireEffectStop;
    public float FrostCoolDown;
    public float FrostWaitTime;
    public float SparkyCoolDown;
    public float MaxSparkyChargingTime;
    public float SparkyShootForce;
    public float SparkyTransScale;
    public float LightningCoolDown;
    public float ExploCoolDown;
    public float BugCircularChoosingTime;
    public float SwitchingParallelTime;
    public float RootedTime;
    public float DisableLightningTime;
    public float LightningAAWait;
    public float LightningAARange;
    public float LightningAAReload;
    public float LightningAAIncAS;
    public float LightningAAWaitB;
    public int LightningCurvePts;
    public float LightningCurveDis;
    public float LightningGlowingTimes;
    public float LightningGlowingWait;
    public float BecomeBearTime;
    public float MaxInBearMode;
    public float BearAttackSpeed;
    public float BearAttackRadius;
    public float BearAttackDis;
    public float BearAttackBasicDamage;
    public float BearThirdAttackDamage;
    public float BearRegenRate;
    public float BearJumpCoolDown;
    public float BearJumpSpeed;
    public float MaxBearJumpDis;
    public float KnockFloorCoolDown;
    public float AnimKnockWait;
    public float Attack5AnimSpeed;
    public float FirstKnockWait;
    public float SecondKnockWait;
    public float ThirdKnockWait;
    public float KnockForwardScaler;
    public float MaxShieldStored;
    public float ShieldBlockPercent;
    public float ShieldRemainTime;
    public float ShieldUpCoolDown;
    public float ShieldDarkerScaler;
    public float ShieldBlockDamagePercent;
    public float CastLightningWaitingTime;
    public float ThunderCastCoolDown;
    public float ThunderCastAddCD;
    public float PlacingTotemCoolDown;
    public float ChargedTotemAmount;
    public float TotemChargedDis;
    public float HealingPlaceCreatedTime;
    public float HealingPlaceAmount;
    public float HealingFrequency;

    [Header("Debug")]
    public int Skill1;
    public int Skill2;
    public int Skill3;
    public int Skill4;
    public int SkillLevel1;
    public int SkillLevel2;
    public int SkillLevel3;
    public int SkillLevel4;

    private UnityEngine.AI.NavMeshAgent Human_naviAgent;
    private UnityEngine.AI.NavMeshAgent Bear_naviAgent;
    private RaycastHit hit;
    private Rigidbody PlayerRb;
    private Rigidbody BearRb;
    private bool Firing;
    private GameObject BulletPrefab;
    private Vector3 FacingTarget;
    private bool Dashing;
    private bool Doing;
    private bool MedkitHealCD;
    private float _DashCD;
    private float OriHP;
    private int[] SkillEvent = {4, 1, 2, 3};
    private KeyCode[] SkillKey = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};
    private Coroutine RSTDoing;
    private bool IsRooted;
    private bool BearMode;

    void Start()
    {
        Human_naviAgent = Human.GetComponent<NavMeshAgent>();
        Bear_naviAgent = Bear.GetComponent<NavMeshAgent>();
        PlayerRb = Human.GetComponent<Rigidbody>();
        BearRb = Bear.GetComponent<Rigidbody>();
        SetGun();
        Init();
    }

    void Update()
    {
        if (!DataManager.Instance.IsPlayerDead) {
            UpdateSkill();
            if (!Switching && !Turning) {
                LocateDestination();
                FaceTarget();
                Attack();
                Skills();
                BioValDetect();
                ToggleInParallel();
            }
            DeadDetect();
            CheckDamaged();
            UpdateControlValue();
            UpdateValue();
        } else {
            // Maybe reset?
        }
    }

    void Init() {
        if (OnDebug) {
            DataManager.Instance.SetPlayerHP(MAXHP);
            DataManager.Instance.SetMAXHP(MAXHP);
            DataManager.Instance.SetBiolanceValue(0);
            DataManager.Instance.SetSkillEvent(0, Skill1);
            DataManager.Instance.SetSkillEvent(1, Skill2);
            DataManager.Instance.SetSkillEvent(2, Skill3);
            DataManager.Instance.SetSkillEvent(3, Skill4);
            DataManager.Instance.SetSkillLevel(0, SkillLevel1);
            DataManager.Instance.SetSkillLevel(1, SkillLevel2);
            DataManager.Instance.SetSkillLevel(0, SkillLevel3);
            DataManager.Instance.SetSkillLevel(3, SkillLevel4);
        }
        Firing = false;
        Dashing = false;
        Doing = false;
        AvoidCasting = false;
        Human.SetActive(true);
        Bear.SetActive(false);
        DataManager.Instance.Scaling = Scaling;
        Human.transform.localScale = new Vector3(HumanScale * Scaling, HumanScale * Scaling, HumanScale * Scaling);
        Bear.transform.localScale = new Vector3(BearScale * Scaling, BearScale * Scaling, BearScale * Scaling);
        transform.position = SpawnPoint;
        Human.transform.position = SpawnPoint;
        Human_naviAgent.isStopped = false;
        DashEffect.SetActive(false);
        MedkitHealCD = true;
        FireEffect.SetActive(false);
        HealEffect.SetActive(false);
        DataManager.Instance.PlayerDead(false);
        OriHP = DataManager.Instance.HP();
        ResetAnimDoing();
    }

    void CheckDamaged() {
        if (DataManager.Instance.ShootOnSB) {
            DataManager.Instance.ShootOnSB = false;
            ChargingTotem(ChargedTotemAmount);
        }
    }

    void UpdateSkill() {
        for (int i = 0; i < 4; i++) {
            SkillEvent[i] = DataManager.Instance.SkillEvent[i];
            DataManager.Instance.SetSkillCD(i, GetCD(SkillEvent[i]));
            DataManager.Instance.SetMAXSkillCD(i, GetMAXCD(SkillEvent[i]));
            DataManager.Instance.SetLightningMode(LightningCast);
        }
    }

    void UpdateControlValue() {
        IsRooted = DataManager.Instance.PlayerIsRooted;
        RootedDetect();
    }

    private bool OriIsRooted;
    void RootedDetect() {
        if (OriIsRooted != IsRooted && OriIsRooted == false) {
            ToggleNavi();
            PlayerAnim.SetBool("Walking", false);
            PlayerAnim.SetInteger("Doing", 4);
            Invoke("ResetAnimDoing", 0.3f);
            Invoke("ResetRooted", DataManager.Instance.RootedTime);
        }
        OriIsRooted = IsRooted;
    }

    void ResetRooted() {
        DataManager.Instance.ToggleRooted();
    }

    float GetCD(int id) {
        switch(id) {
            case 0: return _DashCD;
            case 1: return CurFrostCD;
            case 2: return CurSparkyCD;
            case 3: return CurLightningCD;
            case 4: return CurExploCD;
            case 5: return CurPlacingTotemCD;

            case 101: return CurExploCD;
            case 102: return CurExploCD;
            case 103: return CurExploCD;

            case 201: return CurBearJumpCD;
            case 202: return CurKnockFloorCD;
            case 203: return CurBearShieldCD;

            case 302: return CurThunderCastCD;

            default: return 0.0f;
        }
    }

    float GetMAXCD(int id) {
        switch(id) {
            case 0: return DashCooldown;
            case 1: return FrostCoolDown;
            case 2: return SparkyCoolDown;
            case 3: return LightningCoolDown;
            case 4: return ExploCoolDown;
            case 5: return PlacingTotemCoolDown;

            case 101: return ExploCoolDown;
            case 102: return ExploCoolDown;
            case 103: return ExploCoolDown;

            case 201: return BearJumpCoolDown;
            case 202: return KnockFloorCoolDown;
            case 203: return ShieldUpCoolDown;

            case 302: return LastThunderCastCD;

            default: return 0.0f;
        }
    }

    private bool AvoidCasting;
    void Skills() {
        for (int i = 0; i < 4; i++) {
            if (Input.GetKey(SkillKey[i]) && !CheckSkillState(SkillEvent[i])) {
                if (!AvoidCasting && !Turning) {
                    StartCoroutine(CastSkill(SkillEvent[i], new Vector3(0, 0, 0), 0f, SkillKey[i], i));
                }
            }
        }
    }

    bool CheckSkillState(int skillNum) {
        if (!LightningCast) {
            switch(skillNum) {
                case 0: return Dashing || IsRooted;
                case 1: return FrostCD;
                case 2: return SparkyCD;
                case 3: return LightningCD;
                case 4: return ExploCD;
                case 5: return PlacingTotemCD;

                case 201: return BearJumpCD;
                case 202: return KnockFloorCD;
                case 203: return BearShieldCD;

                case 302: return ThunderCastCD;
            }
        } else {
            switch(skillNum) {
                case 0: return true;
                case 1: return true;
                case 2: return true;
                case 3: return LightningCD;
                case 4: return true;

                case 201: return true;
                case 202: return true;
                case 203: return true;

                case 302: return ThunderCastCD;
            }
        }
        return true;
    }

    IEnumerator CastSkill(int skillNum, Vector3 Target, float dur, KeyCode key, int keyid) {
        if (dur != 0)
            yield return new WaitForSeconds(dur);
        AvoidCasting = false;
        switch(skillNum) {
            case 0: Dash(Target); break;
            case 1: Frost(Target); break;
            case 2: ShootSparky(key); break;
            case 3: Thunder(); break;
            case 4: ExplosiveBug(keyid, key); break;
            case 5: PlacingTotem(); break;

            case 201: BearJump(); break;
            case 202: BearKnockFloor(); break;
            case 203: BearShield(); break;

            case 302: ThunderCast(); break;
            default: break;
        }
    }

    void UpdateValue() {
        if (BearMode) 
            DataManager.Instance.SetPlayerPos(Bear.transform.position);
        else
            DataManager.Instance.SetPlayerPos(Human.transform.position);
    }

    public Vector3 PlayerPos() {
        return DataManager.Instance.PlayerPos;
    }

    void LocateDestination() {
        if (Doing || IsRooted) {
            PlayerAnim.SetBool("Walking", false);
            BearAnim.SetBool("WalkForward", false);
            return;
        }
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Ground")) {
                    if (BearMode) 
                        Bear_naviAgent.SetDestination(hit.point);
                    else
                        Human_naviAgent.SetDestination(hit.point);
                }
            }
        }
        //audioPlayer.PlayOneShot(walkSE);
        bool IsWalking = true;
        if (BearMode) {
            float dist = Bear_naviAgent.remainingDistance;
            if (dist!=Mathf.Infinity && Bear_naviAgent.pathStatus==UnityEngine.AI.NavMeshPathStatus.PathComplete && Bear_naviAgent.remainingDistance<0.1f)
                IsWalking = false;

            // if (!Bear_naviAgent.pathPending) {
            //     if (Bear_naviAgent.remainingDistance <= Bear_naviAgent.stoppingDistance) {
            //         if (!Bear_naviAgent.hasPath || Bear_naviAgent.velocity.sqrMagnitude == 0f)
            //             IsWalking = false;
            //     }
            // }
            if (Bear_naviAgent.isStopped)
                IsWalking = false;
        } else {
            float dist = Human_naviAgent.remainingDistance;
            if (dist!=Mathf.Infinity && Human_naviAgent.pathStatus==UnityEngine.AI.NavMeshPathStatus.PathComplete && Human_naviAgent.remainingDistance<0.1f)
                IsWalking = false;
            // if (!Human_naviAgent.pathPending) {
            //     if (Human_naviAgent.remainingDistance <= Human_naviAgent.stoppingDistance) {
            //         if (!Human_naviAgent.hasPath || Human_naviAgent.velocity.sqrMagnitude == 0f)
            //             IsWalking = false;
            //     }
            // }
            if (Human_naviAgent.isStopped)
                IsWalking = false;
        }
        PlayerAnim.SetBool("Walking", IsWalking);
        BearAnim.SetBool("WalkForward", IsWalking);
    }

    void FaceTarget() {
        if (BearMode) {
            if (Bear_naviAgent.enabled && Bear_naviAgent.velocity != Vector3.zero && !Bear_naviAgent.isStopped) {
                Bear.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Bear.transform.rotation, Quaternion.LookRotation(Bear_naviAgent.velocity - Vector3.zero), Time.deltaTime * BearRotationSlerp).eulerAngles.y, 0);
            } else if (Bear_naviAgent.enabled && Bear_naviAgent.isStopped && FacingTarget != Vector3.zero) {
                Bear.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Bear.transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * BearRotationSlerp * 2).eulerAngles.y, 0);
            }
        } else {
            if (Human_naviAgent.velocity != Vector3.zero && !Human_naviAgent.isStopped) {
                Human.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Human.transform.rotation, Quaternion.LookRotation(Human_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
            } else if (Human_naviAgent.isStopped && FacingTarget != Vector3.zero) {
                Human.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Human.transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * RotationSlerp * 2).eulerAngles.y, 0);
            }
        }
    }

    void SetGun() {
        if (rightGunBone.childCount > 0)
			Destroy(rightGunBone.GetChild(0).gameObject);
        if (rightGun != null) {
            GameObject newRightGun = (GameObject) Instantiate(rightGun);
            newRightGun.transform.parent = rightGunBone;
            newRightGun.transform.localPosition = Vector3.zero;
            newRightGun.transform.localRotation = Quaternion.Euler(-15, 85, -90);
            Vector3 sc = newRightGun.transform.localScale;
            Vector3 objsc = Human.transform.localScale;
            sc = new Vector3(sc.x*objsc.x, sc.y*objsc.y, sc.z*objsc.z);
            newRightGun.transform.localScale = sc;
        }
    }

    private bool LightningFiring;
    private float CurLightningAttackSpeed;
    private int BearAttackCnt;
    void Attack() {
        if (Doing)
            return;
        if (!BearMode) {
            if (Input.GetKey(KeyCode.A) && !Firing && !LightningCast) {
                AvoidCasting = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 Target = Vector3.zero;
                if (Physics.Raycast(ray, out hit)) {
                    Target = hit.point;
                }
                Firing = true;
                FacingTarget = Target - Human.transform.position;
                ToggleNavi();
                IEnumerator shoot = ShootBullet(Target);
                StartCoroutine(shoot);
                Invoke("ResetFiring", ReloadSpeed);
            } else if (Input.GetKey(KeyCode.A) && !LightningFiring && LightningCast) {
                // Vector3 ShootDir = GetMousePos();
                AvoidCasting = true;
                GameObject Ene = FindNearestEnemy();
                if (Ene == null)
                    return;
                FacingTarget = Ene.transform.position - Human.transform.position;
                FacingTarget.y = 0;
                LightningFiring = true;
                ToggleNavi();
                StartCoroutine(LightningAA(Ene));
                Invoke("ResetLightningFiring", CurLightningAttackSpeed);
                CurLightningAttackSpeed += LightningAAIncAS;
            }
        } else {
            if (Input.GetKey(KeyCode.A) && !Firing) {
                AvoidCasting = true;
                Vector3 Target = GetMousePos();
                Firing = true;
                FacingTarget = Target - Bear.transform.position;
                ToggleNavi();
                if (BearAttackCnt == 0) {
                    BearAnim.SetTrigger("Attack1");
                } else if (BearAttackCnt == 1) {
                    BearAnim.SetTrigger("Attack2");
                } else {
                    BearAnim.SetTrigger("Attack3");
                }
                BearAttackCnt++;
                BearAttackCnt %= 3;
                Invoke("CheckHit", 0.5f);
                Invoke("ToggleNavi", 0.8f);
                Invoke("ResetFiring", BearAttackSpeed);
                Invoke("ResetAvoidCasting", 0.8f);
            }
        }
    }

    GameObject FindNearestEnemy() {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float ShortestDis = LightningAARange;
        GameObject ClosetEnemy = null;
        foreach (var obj in Enemies) {
            Vector3 a = obj.transform.root.position;
            Vector3 b = Human.transform.position;
            a.y = 0;
            b.y = 0;
            float dis = Vector3.Distance(a, b);
            if (dis <= LightningAARange && ClosetEnemy == null) {
                ClosetEnemy = obj.transform.root.gameObject;
            } else if (dis < ShortestDis) {
                ClosetEnemy = obj.transform.root.gameObject;
                ShortestDis = dis;
            }
        }
        return ClosetEnemy;
    }

    void CheckHit() {
        float damage;
        if (BearAttackCnt != 0)
            damage = BearAttackBasicDamage;
        else
            damage = BearThirdAttackDamage;
        RaycastHit[] hit;
        Dictionary<int, int> enemies = new Dictionary<int, int>();
        hit = Physics.SphereCastAll(BearFront.position, BearAttackRadius * Scaling, Bear.transform.forward, BearAttackDis * Scaling);
        foreach (var obj in hit) {
            if (obj.collider.CompareTag("Enemy")) {
                int hash = obj.transform.root.GetHashCode();
                if (!enemies.ContainsKey(hash)) {
                    Debug.Log("Bear Attack On: " + obj.transform.root.name);
                    DataManager.Instance.takedamage(obj.transform.root, damage);
                    DataManager.Instance.HealPlayer(damage * BearRegenRate * 0.01f);
                    enemies.Add(hash, 1);
                }
            }
        }
    }

    void ResetLightningFiring() {
        LightningFiring = false;
        ResetLE = false;
    }

    IEnumerator LightningAA(GameObject Enemy) {
        while (PlayerAnim.IsInTransition(0)) {
            yield return null;
        }
        PlayerAnim.SetInteger("Doing", 8);
        Invoke("ResetAnimDoing", 0.2f);
        yield return new WaitForSeconds(LightningAAWait);
        for (var i = LightningEmt.transform.childCount - 1; i >= 0; i--) {
            Destroy(LightningEmt.transform.GetChild(i).gameObject);
        }
        StartCoroutine(LightningInstantiate(Enemy));
        if (LightningManager.Instance != null)
            LightningManager.Instance.HitOn(Enemy.transform);
        Invoke("ToggleNavi", LightningAAWaitB);
        AvoidCasting = false;
    }

    IEnumerator LightningInstantiate(GameObject Target) {
        var curvept = new Vector3[LightningCurvePts+2];
        var v2 = new float[LightningCurvePts+2];
        float[] lenV = new float[LightningCurvePts+2];
        lenV[0] = 0;
        lenV[LightningCurvePts+1] = 1;
        for (var i = 1; i < LightningCurvePts+1; i++) 
            lenV[i] = UnityEngine.Random.value;
        Array.Sort(lenV);
        for (var i = 1; i < LightningCurvePts+1; i++)
            v2[i] = UnityEngine.Random.Range(0, 360);
        GameObject LightningPrefab = Instantiate(Lightning, rightHand.position, Quaternion.identity);
        LineRenderer LR = LightningPrefab.GetComponent<LineRenderer>();
        int glowtimes = 0;
        int StopGlow = 5;
        while (glowtimes < LightningGlowingTimes) {
            Vector3 TargetP = Target.transform.position;
            TargetP.y += HumanScale * Scaling;
            Vector3 Toward = TargetP - rightHand.position;
            Vector3 TowardNorm = Toward.normalized;
            float alpha = UnityEngine.Random.value;
            Color s = LR.startColor;
            Color e = LR.endColor;
            if (glowtimes % StopGlow == 0) {
                s.a = 0;
                e.a = 0;
            } else {
                s.a = alpha;
                e.a = alpha;
            }
            LR.startColor = s;
            LR.endColor = e;
            Vector3 Basis = Vector3.Cross(Toward, transform.up);
            Basis *= LightningCurveDis / Basis.magnitude;
            Vector3[] curvept2 = new Vector3[LightningCurvePts+2];
            curvept[0] = rightHand.position;
            curvept[LightningCurvePts+1] = Target.transform.position;
            for (var i = 1; i < LightningCurvePts+1; i++) {
                Vector3 BasisR = Quaternion.AngleAxis(v2[i], TowardNorm) * Basis;
                curvept[i] = rightHand.position + Toward * lenV[i] + BasisR;
            }
            LR.positionCount = LightningCurvePts+2;
            LR.SetPositions(curvept);
            glowtimes++;
            yield return new WaitForSeconds(LightningGlowingWait);
        }
        Destroy(LightningPrefab);
    }

    void DisableFireEffect() {
        FireEffect.SetActive(false);
    }

    IEnumerator ShootBullet(Vector3 Target) {
        while (PlayerAnim.IsInTransition(0)) {
            yield return null;
        }
        PlayerAnim.SetInteger("Doing", 1);
        Invoke("ResetAnimDoing", 0.1f);
        yield return new WaitForSeconds(ShootWaitingTime);
        audioPlayer.PlayOneShot(shootSE);
        Vector3 ShootDir = Target - rightHand.position;
        ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        FireEffect.SetActive(true);
        BulletPrefab = Instantiate(Bullet, rightHand.position, Quaternion.identity);
        BulletPrefab.transform.localScale *= Scaling;
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
        Invoke("DisableFireEffect", FireEffectStop);
        Invoke("ToggleNavi", AttackAnimWait);
        Invoke("ResetAvoidCasting", AttackAnimWait);
    }

    void ToggleNavi() {
        if (!BearMode) {
            if (Human_naviAgent.enabled) {
                if (Human_naviAgent.isStopped == true) {
                    Human_naviAgent.isStopped = false;
                } else {
                    Human_naviAgent.ResetPath();
                    Human_naviAgent.isStopped = true;
                }
            }
        } else {
            if (Bear_naviAgent.enabled) {
                if (Bear_naviAgent.isStopped == true) {
                    Bear_naviAgent.isStopped = false;
                } else {
                    Bear_naviAgent.ResetPath();
                    Bear_naviAgent.isStopped = true;
                }
            }
        }
    }

    void ActiveNavi() {
        if (!BearMode)
            Human_naviAgent.isStopped = false;
        else 
            Bear_naviAgent.isStopped = false;
    }

    void DeactiveNavi() {
        if (!BearMode)
            Human_naviAgent.isStopped = true;
        else 
            Bear_naviAgent.isStopped = true;
    }

    void ResetAnimDoing() {
        if (DataManager.Instance.IsPlayerDead == false)
            PlayerAnim.SetInteger("Doing", 0);
    }

    void ResetFiring() {
        Firing = false;
    }

    Vector3 GetMousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }

    void Dash(Vector3 Target) {
        Dashing = true;
        DashEffect.SetActive(true);
        PlayerAnim.SetInteger("Doing", 2);
        Vector3 dir = GetMousePos() - Human.transform.position;
        ToggleNavi();
        dir = new Vector3(dir.x, 0f, dir.z);
        dir = dir.normalized;
        FacingTarget = dir;
        Doing = true;
        IEnumerator dashMoving = DashMoving(dir);
        StartCoroutine(dashMoving);
        StartCoroutine(CoolDownCal(DashCooldown, (returnVal1, returnVal2) => {
            _DashCD = returnVal1;
            Dashing = returnVal2;
        }));
        audioPlayer.PlayOneShot(dashSE);
    }

    void ResetDoing(float time) {
        if (RSTDoing != null)
            StopCoroutine(RSTDoing);
        RSTDoing = StartCoroutine(_ResetDoing(time));
    }

    IEnumerator _ResetDoing(float time) {
        yield return new WaitForSeconds(time);
        Doing = false;
    }

    IEnumerator DashMoving(Vector3 dir) {
        float DashAmount = 0f;
        while (DashAmount < DashDistance) {
            Human.transform.position += Time.deltaTime * DashSpeed * dir;
            DashAmount += Time.deltaTime * DashSpeed;
            yield return null;
        }
        ResetAnimDoing();
        ResetDoing(0f);
        ToggleNavi();
        Invoke("DashEffectDisabled", 0.3f);
        DataManager.Instance.PlayerOnHit(50f);
        DataManager.Instance.SetBiolanceValue(10f);
    }

    void DashEffectDisabled() {
        DashEffect.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Medkit") && !LightningCast) {
            if (MedkitHealCD) {
                audioPlayer.PlayOneShot(healSE);
                Destroy(other.gameObject.transform.parent.gameObject);
                HealEffect.SetActive(true);
                MedkitHealCD = false;
                DataManager.Instance.HealPlayer(MedkitHealHP);
                Invoke("ResetMedkitHealCD", 0.2f);
                Invoke("HealEffectDisabled", 0.8f);
            }
        }
    }

    void HealEffectDisabled() {
        HealEffect.SetActive(false);
    }

    void ResetMedkitHealCD() {
        MedkitHealCD = true;
    }

    void DeadDetect() {
        if (DataManager.Instance.IsPlayerDead == false) {
            float CurHP = DataManager.Instance.HP();
            if (CurHP <= 0) {
                audioPlayer.PlayOneShot(deadSE);
                DataManager.Instance.PlayerDead(true);
                PlayerAnim.SetInteger("Doing", 3);
                PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
                Doing = true;
                ToggleNavi();
                Invoke("LoadLoseScene", 4f);
            } else {
                if (CurHP < OriHP) {
                    audioPlayer.PlayOneShot(hurtSE);
                    Instantiate(DamagedEffect, Human.transform.position, Quaternion.identity);
                    PlayerAnim.SetInteger("Doing", 4);
                    Invoke("ResetAnimDoing", 0.1f);
                }
                OriHP = CurHP;
            }
        }
    }

    void LoadLoseScene() {
        if (OnDebug)
            Init();
        else
            SceneManager.LoadScene("Lose");
    }

    public void Reset() {
        Init();
    }

    private bool FrostCD = false;
    private float CurFrostCD;
    void Frost(Vector3 SubTarget) {
        Vector3 Target = GetMousePos() - Human.transform.position;
        ToggleNavi();
        FacingTarget = Target;
        FrostCD = true;
        PlayerAnim.SetInteger("Doing", 5);
        Doing = true;
        Target.y = 0;
        StartCoroutine(ShootFrost(Target, FrostWaitTime));
        audioPlayer.PlayOneShot(frostSE);
        StartCoroutine(CoolDownCal(FrostCoolDown, (returnVal1, returnVal2) => {
            CurFrostCD = returnVal1;
            FrostCD = returnVal2;
        }));
        ResetDoing(FrostWaitTime + 0.2f);
        Invoke("ResetAnimDoing", 0.1f);
        Invoke("ToggleNavi", FrostWaitTime + 0.2f);
    }

    IEnumerator ShootFrost(Vector3 Target, float wait) {
        yield return new WaitForSeconds(wait);
        Vector3 posi = (rightHand.position + leftHand.position) / 2.0f;
        posi.y = Human.transform.position.y + 0.2f;
        GameObject FrostPre = Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target));
        FrostPre.transform.localScale *= Scaling;
    }

    IEnumerator CoolDownCal(float coolDown, System.Action<float, bool> callback) {
        float _CD = coolDown;
        while (_CD > 0) {
            _CD -= 0.05f;
            callback(_CD, true);
            yield return new WaitForSeconds(0.05f);
        }
        _CD = 0;
        callback(0f, false);
        yield return null;
    }

    private bool LastBreathe;
    void BioValDetect() {
        float BioVal = DataManager.Instance.BiolanceValue;
        if (!Turning && !LastBreathe && !BearMode && BioVal >= 100f) {
            BecomeBear();
            DataManager.Instance.BearTime++;
            Turning = true;
            if (DataManager.Instance.BearTime >= MaxInBearMode)
                LastBreathe = true;
        }
    }

    private bool Turning;
    void BecomeBear() {
        AvoidCasting = true;
        BearAttackCnt = 0;
        if (LightningCast)
            ThunderReset();
        StartCoroutine(BecomeBearAnimation());
        Invoke("BecomeHuman", BecomeBearTime);
    }

    void BecomeHuman() {
        StartCoroutine(BecomeHumanAnimation());
    }

    IEnumerator BecomeBearAnimation() {
        CancelInvoke("ToggleNavi");
        CancelInvoke("ResetAnimDoing");
        DeactiveNavi();
        ResetAnimDoing();
        PlayerAnim.SetInteger("Doing", 10);
        float dur = 0;
        Turning = true;
        GameObject Chprefab = Instantiate(ChargeEffect, Human.transform.position, Quaternion.identity);
        Chprefab.transform.localScale *= Scaling;
        while (dur < TurnIntoDuration) {
            dur += Time.deltaTime;
            yield return null;
        }
        Destroy(Chprefab, 0.5f);
        Human.SetActive(false);
        PlayerAnim.SetInteger("Doing", 0);
        Bear.transform.position = Human.transform.position;
        Bear.SetActive(true);
        BearMode = true;
        Bear.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        BearAnim.SetTrigger("Buff");
        Vector3 TurnInto = new Vector3(BearScale, BearScale, BearScale);
        TurnInto *= Scaling;
        while (Bear.transform.localScale.x < BearScale * Scaling * 0.95f) {
            Bear.transform.localScale = Vector3.Slerp(Bear.transform.localScale, TurnInto, Time.deltaTime*TurnIntoScaler);
            yield return null;
        }
        Bear.transform.localScale = TurnInto;
        yield return new WaitForSeconds(0.5f);
        DataManager.Instance.InBearMode = true;
        ResetAvoidCasting();
        ActiveNavi();
        BearOnCast = false;
        Turning = false;
        DataManager.Instance.SetSkillEvent(0, 201);
        DataManager.Instance.SetSkillEvent(1, 202);
        DataManager.Instance.SetSkillEvent(2, 203);
        DataManager.Instance.SetSkillEvent(3, 204);
    }

    IEnumerator BecomeHumanAnimation() {
        while (BearOnCast)
            yield return null;
        CancelInvoke("ToggleNavi");
        CancelInvoke("ResetAnimDoing");
        DeactiveNavi();
        Vector3 TurnInto = new Vector3(HumanScale*BearHumanScaleRatio*Scaling, HumanScale*BearHumanScaleRatio*Scaling, HumanScale*BearHumanScaleRatio*Scaling);
        while (Bear.transform.localScale.x > HumanScale*BearHumanScaleRatio*Scaling * 1.1f) {
            Bear.transform.localScale = Vector3.Slerp(Bear.transform.localScale, TurnInto, Time.deltaTime*TurnIntoScaler);
            yield return null;
        }
        DataManager.Instance.SetBiolanceValue(0f);
        Bear.SetActive(false);
        Human.transform.position = Bear.transform.position;
        Human.SetActive(true);
        BearMode = false;
        Turning = false;
        Human.transform.localScale = new Vector3(HumanScale*Scaling, HumanScale*Scaling, HumanScale*Scaling);
        yield return new WaitForSeconds(1f);
        ActiveNavi();
        DataManager.Instance.InBearMode = false;
        DataManager.Instance.SetSkillEvent(0, 0);
        DataManager.Instance.SetSkillEvent(1, 1);
        DataManager.Instance.SetSkillEvent(2, 2);
        DataManager.Instance.SetSkillEvent(3, 3);
    }

    private float CurSparkyCD;
    private bool SparkyCD;
    void ShootSparky(KeyCode key) {
        PlayerAnim.SetInteger("Doing", 7);
        ToggleNavi();
        SparkyCD = true;
        FacingTarget = GetMousePos() - Human.transform.position;
        StartCoroutine(SparkyCharge(key));
    }

    IEnumerator SparkyCharge(KeyCode key) {
        float scale = 0;
        float dur = 0;
        AvoidCasting = true;
        yield return new WaitForSeconds(0.1f);
        GameObject ChargingEF;
        ChargingEF = Instantiate(SparkyChargeEffect, (rightHand.position + leftHand.position) / 2.0f, Quaternion.identity);
        Vector3 IntoScale = new Vector3(0.7f, 0.7f, 0.7f);
        while (Input.GetKey(key) && dur < MaxSparkyChargingTime) {
            if (dur > MaxSparkyChargingTime / 3.0f) {
                ChargingEF.transform.localScale = Vector3.Slerp(ChargingEF.transform.localScale, IntoScale, Time.deltaTime * 2.5f);
            }
            scale = ((ChargingEF.transform.localScale.x - 0.3f) / 0.4f) * 0.8f + 0.2f;
            dur += Time.deltaTime;
            yield return null;
        }
        if (scale > 1)
            scale = 1;
        Destroy(ChargingEF);
        ResetAnimDoing();
        GameObject SparkyBullet = Instantiate(Sparky, rightHand.position, Quaternion.identity);
        SparkyBullet.transform.localScale = scale * SparkyTransScale * new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 ShootDir = GetMousePos() - Human.transform.position;
        ShootDir.y = 0f;
        yield return new WaitForSeconds(0.25f);
        FacingTarget = ShootDir;
        SparkyBullet.GetComponent<Rigidbody>().AddForce(ShootDir.normalized * SparkyShootForce);
        StartCoroutine(CoolDownCal(SparkyCoolDown, (returnVal1, returnVal2) => {
            CurSparkyCD = returnVal1;
            SparkyCD = returnVal2;
        }));
        Invoke("ResetAvoidCasting", 0.3f);
        Invoke("ToggleNavi", 0.3f);
    }

    void ResetAvoidCasting() {
        AvoidCasting = false;
    }

    private bool LightningCD;
    private float CurLightningCD;
    private bool LightningCast;
    private GameObject LightningManPrefab;
    private Coroutine lightningAround;
    void Thunder() {          
        if (!LightningCast) {
            ToggleNavi();
            StartCoroutine(TemporarySetMax());
            StartCoroutine(CoolDownCal(DisableLightningTime, (returnVal1, returnVal2) => {
                CurLightningCD = returnVal1;
                LightningCD = returnVal2;
            }));
            LightningManPrefab = Instantiate(LightningMan);
            CurLightningAttackSpeed = LightningAAReload;
            PlayerAnim.SetInteger("Doing", 9);
            ResetLE = false;
            Invoke("ResetAnimDoing", 0.2f);
            Invoke("WaitThunderAnim", DisableLightningTime - 0.2f);
        } else {
            ThunderReset();
        }
    }

    void ThunderReset() {
        LightningMode.SetActive(false);
        LightningCast = false;
        Destroy(LightningManPrefab);
        StopCoroutine(lightningAround);
        for (var i = LightningEmt.transform.childCount - 1; i >= 0; i--) {
            Destroy(LightningEmt.transform.GetChild(i).gameObject);
        }
        StartCoroutine(CoolDownCal(LightningCoolDown, (returnVal1, returnVal2) => {
            CurLightningCD = returnVal1;
            LightningCD = returnVal2;
        }));
        DataManager.Instance.SetSkillEvent(0, 0);
        DataManager.Instance.SetSkillEvent(1, 1);
        DataManager.Instance.SetSkillEvent(2, 5);
    }

    IEnumerator TemporarySetMax() {
        float tmp = LightningCoolDown;
        LightningCoolDown = DisableLightningTime;
        yield return new WaitForSeconds(DisableLightningTime);
        LightningCoolDown = tmp;
    }

    void WaitThunderAnim() {
        LightningCast = true;
        LightningFiring = false;
        LightningMode.SetActive(true);
        DataManager.Instance.SetSkillEvent(0, 301);
        DataManager.Instance.SetSkillEvent(1, 302);
        DataManager.Instance.SetSkillEvent(2, 5);
        CurThunderCastMaxCD = ThunderCastCoolDown;
        LastThunderCastCD = ThunderCastCoolDown;
        TotemCharged = 0;
        ToggleNavi();
        lightningAround = StartCoroutine(LightningAround());
    }

    private GameObject LE;
    private GameObject FullEnergy1;
    private GameObject FullEnergy2;
    private bool ResetLE;
    IEnumerator LightningAround() {
        while (true) {
            while (LightningFiring)
                yield return null;
            if (!ResetLE) {
                LE = Instantiate(LightningEffect, Human.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                LE.transform.parent = LightningEmt.transform;
                ResetLE = true;
                Destroy(LE, 0.5f);
                yield return new WaitForSeconds(0.5f);
                FullEnergy1 = Instantiate(LightningRefill1, Human.transform.position + new Vector3(0f, 0.8f, -0.18f), Quaternion.identity);
                // yield return new WaitForSeconds(0.2f);
                FullEnergy2 = Instantiate(LightningRefill2, Human.transform.position + new Vector3(0f, 0.8f, -0.18f), Quaternion.identity);
                FullEnergy1.transform.parent = LightningEmt.transform;
                FullEnergy2.transform.parent = LightningEmt.transform;
            }
            yield return null;
        }
    }

    private bool ExploCD;
    private float CurExploCD;
    private int ExploLevel;
    void ExplosiveBug(int keyid, KeyCode key) {
        ExploLevel = DataManager.Instance.SkillLevel[4];
        if (ExploLevel == 0) {
            StartCoroutine(CoolDownCal(ExploCoolDown, (returnVal1, returnVal2) => {
                CurExploCD = returnVal1;
                ExploCD = returnVal2;
            }));
            int BugNum = UnityEngine.Random.Range(1, 4);
            PutOutBug(BugNum);
        } else if (ExploLevel == 1) {
            StartCoroutine(ChoosingBug(keyid, key));
        } else if (ExploLevel == 2) {
            StartCoroutine(CoolDownCal(ExploCoolDown, (returnVal1, returnVal2) => {
                CurExploCD = returnVal1;
                ExploCD = returnVal2;
            }));
            PutOutBug(1);
            PutOutBug(2);
            PutOutBug(3);
        }
    }

    void PutOutBug(int num) {
        if (num == 1) {
            Instantiate(ExploBugR, Human.transform.position, Quaternion.identity);
        } else if (num == 2) {
            Instantiate(ExploBugG, Human.transform.position, Quaternion.identity);
        } else if (num == 3) {
            Instantiate(ExploBugB, Human.transform.position, Quaternion.identity);
        }
    }

    private int ChoosingBugNum;
    IEnumerator ChoosingBug(int id, KeyCode key) {
        ChoosingBugNum = UnityEngine.Random.Range(1, 4);
        Coroutine Switching = StartCoroutine(ChangingBugNum(id));
        yield return new WaitForSeconds(0.1f);
        while (true) {
            if (Input.GetKeyDown(key)) {
                StopCoroutine(Switching);
                break;
            }
            yield return null;
        }
        DataManager.Instance.SetSkillEvent(id, 4);
        PutOutBug(ChoosingBugNum);
        StartCoroutine(CoolDownCal(ExploCoolDown, (returnVal1, returnVal2) => {
            CurExploCD = returnVal1;
            ExploCD = returnVal2;
        }));
    }

    IEnumerator ChangingBugNum(int id) {
        while (true) {
            ChoosingBugNum = (ChoosingBugNum + 1) % 3 + 1;
            if (ChoosingBugNum == 1) {
                DataManager.Instance.SetSkillEvent(id, 101);
            } else if (ChoosingBugNum == 2) {
                DataManager.Instance.SetSkillEvent(id, 102);
            } else if (ChoosingBugNum == 3) {
                DataManager.Instance.SetSkillEvent(id, 103);
            }
            yield return new WaitForSeconds(BugCircularChoosingTime);
        }
    }

    private bool OriInParrallel;
    private bool Switching;
    void ToggleInParallel() {
        bool inParallel = DataManager.Instance.InParallel;
        if (OriInParrallel != inParallel) {
            if(inParallel) {
                Human_naviAgent.Warp(new Vector3(Human.transform.position.x, 50, Human.transform.position.z));
            } else {
                Human_naviAgent.Warp(new Vector3(Human.transform.position.x, 0, Human.transform.position.z));
            }
            ToggleNavi();
            Switching = true;
            Invoke("ToggleNavi", SwitchingParallelTime);
            Invoke("ResetSwitching", SwitchingParallelTime);
        }
        OriInParrallel = inParallel;
    }

    void ResetSwitching() {
        Switching = false;
    }

    private bool BearOnCast;
    private bool BearJumpCD;
    private float CurBearJumpCD;
    void BearJump() {
        Doing = true;
        BearJumpCD = true;
        AvoidCasting = true;
        BearOnCast = true;
        ToggleNavi();
        FacingTarget = GetMousePos() - Bear.transform.position;
        StartCoroutine(CoolDownCal(BearJumpCoolDown, (returnVal1, returnVal2) => {
            CurBearJumpCD = returnVal1;
            BearJumpCD = returnVal2;
        }));
        Vector3 Pos = GetMousePos();
        Vector3 JumpDis = new Vector3(Pos.x, 0f, Pos.z) - new Vector3(Bear.transform.position.x, 0f, Bear.transform.position.z);
        if (JumpDis.sqrMagnitude > MaxBearJumpDis*MaxBearJumpDis)
            JumpDis = JumpDis * (MaxBearJumpDis / JumpDis.magnitude);
        StartCoroutine(Jumping(JumpDis));
    }

    IEnumerator Jumping(Vector3 diff) {
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = BearJumpSpeed * BearJumpSpeed + Vector3.Dot(diff, Physics.gravity);
        float discriminant = b * b - gSquared * diff.sqrMagnitude;
        if(discriminant < 0) {
            // Target is too far away to hit at this speed.
            Debug.Log("Too Far");
            ResetBearJumpArgs();
        } else {
            BearAnim.SetBool("Jump", true);
            float discRoot = Mathf.Sqrt(discriminant);
            // Highest shot with the given max speed:
            float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

            // Most direct shot with the given max speed:
            float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

            // Lowest-speed arc available:
            float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(diff.sqrMagnitude * 4f/gSquared));

            float T = T_max;

            Vector3 velocity = diff / T - Physics.gravity * T / 2f;
            Bear_naviAgent.updatePosition = false;
            Bear_naviAgent.updateRotation = false;
            BearRb.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            BearRb.AddForce(velocity, ForceMode.VelocityChange);
            
            Invoke("ResetBearJumpArgs", T);
        }
    }

    void ResetBearJumpArgs() {
        BearAnim.SetBool("Jump", false);
        ToggleNavi();
        Bear_naviAgent.SetDestination(Bear.transform.position);
        Bear_naviAgent.updatePosition = true;
        Bear_naviAgent.updateRotation = true;
        Doing = false;
        BearOnCast = false;
        AvoidCasting = false;
    }

    private bool KnockFloorCD;
    private float CurKnockFloorCD;
    void BearKnockFloor() {
        Doing = true;
        BearOnCast = true;
        AvoidCasting = true;
        ToggleNavi();
        StartCoroutine(CoolDownCal(KnockFloorCoolDown, (returnVal1, returnVal2) => {
            CurKnockFloorCD = returnVal1;
            KnockFloorCD = returnVal2;
        }));
        StartCoroutine(KnockOnFloor());
    }

    IEnumerator KnockOnFloor() {
        FacingTarget = GetMousePos() - Bear.transform.position;
        FacingTarget.y = 0;
        BearAnim.SetFloat("Attack5Speed", 1f);
        BearAnim.SetBool("Attack5", true);
        GameObject Knock;
        yield return new WaitForSeconds(AnimKnockWait);
        BearAnim.SetBool("Attack5", false);
        Vector3 InsPos = Bear.transform.position + Bear.transform.forward * KnockForwardScaler;
        InsPos.y = Bear.transform.position.y + 0.1f;
        Knock = Instantiate(EarthQuakeEffect, InsPos, Quaternion.Euler(90, 0, 0));
        Knock.transform.localScale *= Scaling;
        yield return new WaitForSeconds(FirstKnockWait);
        BearAnim.SetFloat("Attack5Speed", 1f);
        BearAnim.SetBool("Attack5", true);
        yield return new WaitForSeconds(AnimKnockWait);
        yield return new WaitForSeconds(0.2f);
        BearAnim.SetBool("Attack5", false);
        InsPos = Bear.transform.position + Bear.transform.forward * KnockForwardScaler;
        InsPos.y = Bear.transform.position.y + 0.1f;
        Knock = Instantiate(EarthQuakeEffect, InsPos, Quaternion.Euler(90, 0, 0));
        Knock.transform.localScale *= Scaling;
        yield return new WaitForSeconds(SecondKnockWait);
        BearAnim.SetFloat("Attack5Speed", Attack5AnimSpeed);
        BearAnim.SetBool("Attack5", true);
        yield return new WaitForSeconds(AnimKnockWait*(1/Attack5AnimSpeed)+0.2f);
        BearAnim.SetBool("Attack5", false);
        InsPos = Bear.transform.position + Bear.transform.forward * KnockForwardScaler;
        InsPos.y = Bear.transform.position.y + 0.1f;
        Knock = Instantiate(EarthQuakeEffect, InsPos, Quaternion.Euler(90, 0, 0));
        Knock.transform.localScale *= Scaling;
        BearAnim.SetFloat("Attack5Speed", 1f);
        yield return new WaitForSeconds(ThirdKnockWait);
        Doing = false;
        AvoidCasting = false;
        BearOnCast = false;
        ToggleNavi();
    }

    private bool BearShieldCD;
    private float CurBearShieldCD;
    void BearShield() {
        BearOnCast = true;
        DataManager.Instance.ShieldUp = true;
        DataManager.Instance.ShieldStored = 0f;
        DataManager.Instance.ShieldBlockPer = ShieldBlockPercent;
        DataManager.Instance.ShieldBlockDamagePer = ShieldBlockDamagePercent;
        DataManager.Instance.MaxShieldStored = MaxShieldStored;
        StartCoroutine(CoolDownCal(ShieldUpCoolDown, (returnVal1, returnVal2) => {
            CurBearShieldCD = returnVal1;
            BearShieldCD = returnVal2;
        }));
        StartCoroutine(ShieldBreak());
    }

    IEnumerator ShieldBreak() {
        Vector3 Pos = Bear.transform.position + new Vector3(0f, 1f, 0f);
        GameObject ShieldPrefab = Instantiate(Shield, Pos, Quaternion.identity);
        ShieldPrefab.transform.parent = BearShieldUp;
        ShieldPrefab.transform.localScale *= Scaling;
        float time = 0;
        while(time < ShieldRemainTime) {
            time += Time.deltaTime;
            ParticleSystem.MainModule ps = ShieldPrefab.GetComponent<ParticleSystem>().main;
            Color c = ps.startColor.color;
            c.r *= ShieldDarkerScaler;
            c.g *= ShieldDarkerScaler;
            c.b *= ShieldDarkerScaler;
            ps.startColor = Color.Lerp(ps.startColor.color, c, Time.deltaTime);
            yield return null;
        }
        for (var i = BearShieldUp.transform.childCount - 1; i >= 0; i--) {
            Destroy(BearShieldUp.transform.GetChild(i).gameObject);
        }
        GameObject Break = Instantiate(ShieldBreakCircle, Bear.transform.position, Quaternion.identity);
        Break.transform.localScale *= Scaling;
        DataManager.Instance.ShieldUp = false;
        BearOnCast = false;
    }

    private bool ThunderCastCD;
    private float CurThunderCastCD;
    private float CurThunderCastMaxCD;
    private float LastThunderCastCD;
    void ThunderCast() {
        Doing = true;
        ToggleNavi();
        AvoidCasting = true;
        StartCoroutine(CoolDownCal(CurThunderCastMaxCD, (returnVal1, returnVal2) => {
            CurThunderCastCD = returnVal1;
            ThunderCastCD = returnVal2;
        }));
        LastThunderCastCD = CurThunderCastMaxCD;
        CurThunderCastMaxCD += ThunderCastAddCD;
        Vector3 Target = GetMousePos();
        FacingTarget = Target - Human.transform.position;
        FacingTarget.y = 0f;
        StartCoroutine(CastLightning(GetMousePos()));
    }

    IEnumerator CastLightning(Vector3 Target) {
        PlayerAnim.SetInteger("Doing", 8);
        Invoke("ResetAnimDoing", 0.3f);
        yield return new WaitForSeconds(CastLightningWaitingTime);
        Vector3 ShootDir = Target - (rightHand.position + leftHand.position) / 2;
        ShootDir.y = 0;
        ShootDir = ShootDir.normalized;
        GameObject LightningB = Instantiate(LightningBullet, (rightHand.position + leftHand.position) / 2, Quaternion.LookRotation(ShootDir));
        LightningB.transform.localScale *= Scaling;
        Invoke("ToggleNavi", 0.7f);
        Doing = false;
        AvoidCasting = false;
    }

    private bool PlacingTotemCD;
    private float CurPlacingTotemCD;
    private float TotemCharged;
    private GameObject TotemPrefab;
    void PlacingTotem() {
        if (TotemPrefab != null) {
            Destroy(TotemPrefab);
        }
        StartCoroutine(CoolDownCal(PlacingTotemCoolDown, (returnVal1, returnVal2) => {
            CurPlacingTotemCD = returnVal1;
            PlacingTotemCD = returnVal2;
        }));
        TotemPrefab = Instantiate(Totem, Human.transform.position, Quaternion.identity);
        TotemCharged = 0;
    }

    private bool HealingPlaceCreated;
    void ChargingTotem(float amount) {
        if (TotemPrefab == null)
            return;
        if (HealingPlaceCreated)
            return;
        if (LightningCast) {

        } else {
            if (Vector3.Distance(TotemPrefab.transform.position, Human.transform.position) <= TotemChargedDis) {
                TotemCharged += amount;
            }
            if (TotemCharged >= 100f) {
                HealingPlaceCreated = true;
                StartCoroutine(CreateHealingPlace());
                TotemCharged = 0;
            }
        }
    }

    IEnumerator CreateHealingPlace() {
        float dur = 0;
        while (!LightningCast && dur < HealingPlaceCreatedTime) {
            if (Vector3.Distance(TotemPrefab.transform.position, Human.transform.position) <= TotemChargedDis)
                DataManager.Instance.HealPlayer(HealingPlaceAmount);
            dur += HealingFrequency;
            yield return new WaitForSeconds(HealingFrequency);
        }
        HealingPlaceCreated = false;
    }

    private bool RandomThunderCD;
    private float CurRandomThunderCD;
    private float CurRandomThunderMaxCD;
    private float LastRandomThunderCD; 
    void RandomThunder() {
        if (TotemPrefab == null)
            return;
        
    }
}