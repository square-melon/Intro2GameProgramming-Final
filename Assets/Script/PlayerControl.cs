using System.Collections;
using System.Collections.Generic;
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
    public Vector3 SpawnPoint;
    public float RotationSlerp;
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
    public float DisableLightningTime;
    public float CastLightningWaitingTime;
    public float LightningBasicAS;
    public float LightningAddAS;
    public float ExploCoolDown;
    public float BugCircularChoosingTime;
    public float SwitchingParallelTime;

    [Header("Debug")]
    public int Skill1;
    public int Skill2;
    public int Skill3;
    public int Skill4;
    public int SkillLevel1;
    public int SkillLevel2;
    public int SkillLevel3;
    public int SkillLevel4;

    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private RaycastHit hit;
    private Animator PlayerAnim;
    private Rigidbody PlayerRb;
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

    void Start()
    {
        m_naviAgent = GetComponent<NavMeshAgent>();
        PlayerRb = GetComponent<Rigidbody>();
        PlayerAnim = GetComponent<Animator>();
        SetGun();
        Init();
    }

    void Update()
    {
        if (!DataManager.Instance.IsPlayerDead) {
            UpdateSkill();
            if (!Switching) {
                LocateDestination();
                FaceTarget();
                Attack();
                Skills();
                BioValDetect();
                ToggleInParallel();
            }
            DeadDetect();
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
            DataManager.Instance.SetSkillLevel(2, SkillLevel3);
            DataManager.Instance.SetSkillLevel(3, SkillLevel4);
        }
        Firing = false;
        Dashing = false;
        Doing = false;
        AvoidCasting = false;
        transform.position = SpawnPoint;
        m_naviAgent.isStopped = false;
        DashEffect.SetActive(false);
        MedkitHealCD = true;
        FireEffect.SetActive(false);
        HealEffect.SetActive(false);
        DataManager.Instance.PlayerDead(false);
        OriHP = DataManager.Instance.HP();
        ResetAnimDoing();
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
        if (OriIsRooted != IsRooted);
    }

    float GetCD(int id) {
        switch(id) {
            case 0: return _DashCD;
            case 1: return CurFrostCD;
            case 2: return CurSparkyCD;
            case 3: return CurLightningCD;
            case 4: return CurExploCD;

            case 101: return CurExploCD;
            case 102: return CurExploCD;
            case 103: return CurExploCD;
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

            case 101: return ExploCoolDown;
            case 102: return ExploCoolDown;
            case 103: return ExploCoolDown;
            default: return 0.0f;
        }
    }

    private bool AvoidCasting;
    void Skills() {
        for (int i = 0; i < 4; i++) {
            if (Input.GetKey(SkillKey[i]) && !CheckSkillState(SkillEvent[i])) {
                if (BioLevel == 3) {
                    if (!AvoidCasting) {
                        AvoidCasting = true;
                        Vector3 EffectPos = transform.position;
                        EffectPos.y += 0.5f;
                        EffectPos.z -= 0.2f;
                        Instantiate(ChargeEffect, EffectPos, Quaternion.identity);
                        Vector3 Target = GetMousePos() - transform.position;
                        FacingTarget = Target;
                        PlayerAnim.SetInteger("Doing", 6);
                        StartCoroutine(CastSkill(SkillEvent[i], Target, 1.5f, SkillKey[i], i));
                    }
                } else {
                    if (!AvoidCasting) {
                        StartCoroutine(CastSkill(SkillEvent[i], new Vector3(0, 0, 0), 0f, SkillKey[i], i));
                    }
                }
            }
        }
    }

    bool CheckSkillState(int skillNum) {
        if (!LightningCast) {
            switch(skillNum) {
                case 0: return Dashing;
                case 1: return FrostCD;
                case 2: return SparkyCD;
                case 3: return LightningCD;
                case 4: return ExploCD;
            }
        } else {
            switch(skillNum) {
                case 0: return true;
                case 1: return true;
                case 2: return true;
                case 3: return LightningCD;
                case 4: return true;
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
            default: break;
        }
    }

    void UpdateValue() {
        DataManager.Instance.SetPlayerPos(transform.position);
    }

    void LocateDestination() {
        if (Doing) {
            return;
        }
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Ground")) {
                    m_naviAgent.SetDestination(hit.point);
                }
            }
        }
        //audioPlayer.PlayOneShot(walkSE);
        bool IsWalking = true;
        if (!m_naviAgent.pathPending) {
            if (m_naviAgent.remainingDistance <= m_naviAgent.stoppingDistance) {
                if (!m_naviAgent.hasPath || m_naviAgent.velocity.sqrMagnitude == 0f)
                    IsWalking = false;
            }
        }
        if (m_naviAgent.isStopped)
            IsWalking = false;
        PlayerAnim.SetBool("Walking", IsWalking);
    }

    void FaceTarget() {
        if (m_naviAgent.velocity != Vector3.zero && !m_naviAgent.isStopped) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
        } else if (m_naviAgent.isStopped && FacingTarget != Vector3.zero) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * RotationSlerp * 2).eulerAngles.y, 0);
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
            Vector3 objsc = transform.localScale;
            sc = new Vector3(sc.x*objsc.x, sc.y*objsc.y, sc.z*objsc.z);
            newRightGun.transform.localScale = sc;
        }
    }

    private bool LightningFiring;
    private float CurLightningAttackSpeed;
    void Attack() {
        if (Input.GetKey(KeyCode.A) && !Firing && !LightningCast) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 Target = Vector3.zero;
            if (Physics.Raycast(ray, out hit)) {
                Target = hit.point;
            }
            Firing = true;
            FacingTarget = Target - transform.position;
            ToggleNavi();
            IEnumerator shoot = ShootBullet(Target);
            StartCoroutine(shoot);
            Invoke("ResetFiring", ReloadSpeed);
        } else if (Input.GetKey(KeyCode.A) && !LightningFiring && LightningCast) {
            Vector3 ShootDir = GetMousePos();
            LightningFiring = true;
            FacingTarget = ShootDir - transform.position;
            ToggleNavi();
            StartCoroutine(CastLightning(ShootDir));
            Invoke("ResetLightningFiring", CurLightningAttackSpeed);
            CurLightningAttackSpeed += LightningAddAS;
        }
    }

    void ResetLightningFiring() {
        LightningFiring = false;
        ResetLE = false;
    }

    IEnumerator CastLightning(Vector3 Target) {
        while (PlayerAnim.IsInTransition(0)) {
            yield return null;
        }
        PlayerAnim.SetInteger("Doing", 8);
        Invoke("ResetAnimDoing", 0.1f);
        yield return new WaitForSeconds(CastLightningWaitingTime);
        for (var i = LightningEmt.transform.childCount - 1; i >= 0; i--) {
            Destroy(LightningEmt.transform.GetChild(i).gameObject);
        }
        Vector3 ShootDir = Target - (rightHand.position + leftHand.position) / 2;
        ShootDir.y = 0;
        ShootDir = ShootDir.normalized;
        Instantiate(LightningBullet, (rightHand.position + leftHand.position) / 2, Quaternion.LookRotation(ShootDir));
        Invoke("ToggleNavi", 0.15f);
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
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
        Invoke("DisableFireEffect", FireEffectStop);
        Invoke("ToggleNavi", 0.15f);
    }

    void ToggleNavi() {
        if (m_naviAgent.isStopped == true) {
            m_naviAgent.isStopped = false;
        } else {
            m_naviAgent.ResetPath();
            m_naviAgent.isStopped = true;
        }
    }

    void ResetAnimDoing() {
        if (DataManager.Instance.IsPlayerDead == false)
            PlayerAnim.SetInteger("Doing", 0);
    }

    void ResetFiring() {
        Firing = false;
    }

    public Vector3 PlayerPos() {
        return transform.position;
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
        Vector3 dir = GetMousePos() - transform.position;
        ToggleNavi();
        if (BioLevel == 3) {
            dir = Target;
        }
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
            transform.position += Time.deltaTime * DashSpeed * dir;
            DashAmount += Time.deltaTime * DashSpeed;
            yield return null;
        }
        ResetAnimDoing();
        ResetDoing(0f);
        ToggleNavi();
        Invoke("DashEffectDisabled", 0.3f);
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
                    Instantiate(DamagedEffect, transform.position, Quaternion.identity);
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
        Vector3 Target = GetMousePos() - transform.position;
        if (BioLevel == 3) {
            Target = SubTarget;
        }
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
        posi.y = transform.position.y + 0.2f;
        Instantiate(FrostBeam, posi, Quaternion.LookRotation(Target));
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

    private int BioLevel;
    void BioValDetect() {
        float BioVal = DataManager.Instance.BiolanceValue;
        if (BioVal >= 90f) {
            BioLevel = 3;
        } else if (BioVal >= 60f) {
            BioLevel = 2;
        } else if (BioVal >= 20f) {
            BioLevel = 1;
        } else {
            BioLevel = 0;
        }
    }

    private float CurSparkyCD;
    private bool SparkyCD;
    void ShootSparky(KeyCode key) {
        PlayerAnim.SetInteger("Doing", 7);
        ToggleNavi();
        SparkyCD = true;
        FacingTarget = GetMousePos() - transform.position;
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
        Vector3 ShootDir = GetMousePos() - transform.position;
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
            CurLightningAttackSpeed = LightningBasicAS;
            PlayerAnim.SetInteger("Doing", 9);
            ResetLE = false;
            Invoke("ResetAnimDoing", 0.2f);
            Invoke("WaitThunderAnim", DisableLightningTime - 0.2f);
        } else {
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
        }
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
                LE = Instantiate(LightningEffect, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                LE.transform.parent = transform;
                ResetLE = true;
                Destroy(LE, 0.5f);
                yield return new WaitForSeconds(0.5f);
                FullEnergy1 = Instantiate(LightningRefill1, transform.position + new Vector3(0f, 0.8f, -0.18f), Quaternion.identity);
                // yield return new WaitForSeconds(0.2f);
                FullEnergy2 = Instantiate(LightningRefill2, transform.position + new Vector3(0f, 0.8f, -0.18f), Quaternion.identity);
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
            int BugNum = Random.Range(1, 4);
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
            Instantiate(ExploBugR, transform.position, Quaternion.identity);
        } else if (num == 2) {
            Instantiate(ExploBugG, transform.position, Quaternion.identity);
        } else if (num == 3) {
            Instantiate(ExploBugB, transform.position, Quaternion.identity);
        }
    }

    private int ChoosingBugNum;
    IEnumerator ChoosingBug(int id, KeyCode key) {
        ChoosingBugNum = Random.Range(1, 4);
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
}