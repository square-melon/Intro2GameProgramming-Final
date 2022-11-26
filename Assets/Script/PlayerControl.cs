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
    public GameObject rightGun;
    public GameObject Bullet;
    public GameObject FrostBeam;
    public Transform ShooterPoint;
    public GameObject DashEffect;
    public GameObject FireEffect;
    public GameObject HealEffect;
    public GameObject DamagedEffect;
    public GameObject ChargeEffect;
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
    private int[] SkillEvent = {0, 1, 0, 0};
    private KeyCode[] SkillKey = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};
    private Coroutine RSTDoing;

    void Start()
    {
        m_naviAgent = GetComponent<NavMeshAgent>();
        PlayerRb = GetComponent<Rigidbody>();
        PlayerAnim = GetComponent<Animator>();
        DataManager.Instance.SetMAXDashCD(DashCooldown);
        SetGun();
        Init();
    }

    void Update()
    {
        if (!DataManager.Instance.IsPlayerDead) {
            LocateDestination();
            FaceTarget();
            Attack();
            UpdateValue();
            Skills();
            DeadDetect();
            BioValDetect();
        } else {
            // Maybe reset?
        }
    }

    void Init() {
        if (OnDebug) {
            DataManager.Instance.SetPlayerHP(MAXHP);
            DataManager.Instance.SetMAXHP(MAXHP);
            DataManager.Instance.SetBiolanceValue(0);
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
        DataManager.Instance.SetDashCD(0);
        OriHP = DataManager.Instance.HP();
        ResetAnimDoing();
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
                        ToggleNavi();
                        PlayerAnim.SetInteger("Doing", 6);
                        StartCoroutine(CastSkill(SkillEvent[i], Target, 1.5f));
                    }
                } else {
                    ToggleNavi();
                    StartCoroutine(CastSkill(SkillEvent[i], new Vector3(0, 0, 0), 0f));
                }
            }
        }
    }

    bool CheckSkillState(int skillNum) {
        switch(skillNum) {
            case 0: return Dashing;
            case 1: return FrostCD;
        }
        return true;
    }

    IEnumerator CastSkill(int skillNum, Vector3 Target, float dur) {
        if (dur != 0)
            yield return new WaitForSeconds(dur);
        AvoidCasting = false;
        switch(skillNum) {
            case 0: Dash(Target); break;
            case 1: Frost(Target); break;
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

    void Attack() {
        if (Input.GetKey(KeyCode.A) && !Firing) {
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
        }
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
        Vector3 ShootDir = Target - ShooterPoint.position;
        ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        FireEffect.SetActive(true);
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
        Invoke("DisableFireEffect", FireEffectStop);
        Invoke("ToggleNavi", 0.15f);
        Invoke("DisableFireEffect", FireEffectStop);
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
            DataManager.Instance.SetDashCD(returnVal1);
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
        if (other.gameObject.CompareTag("Medkit")) {
            if (MedkitHealCD) {
                audioPlayer.PlayOneShot(healSE);
                DataManager.Instance.IncreaseBiolanceValue(46f);
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

    public float DashCD() {
        return _DashCD;
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
}
