using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Transform rightGunBone;
    public GameObject rightGun;
    public GameObject Bullet;
    public Transform ShooterPoint;
    public GameObject DashEffect;
    public GameObject FireEffect;
    public GameObject HealEffect;
    public GameObject DamagedEffect;
    public AudioSource audioPlayer;
    public AudioClip shootSE;
    public AudioClip walkSE;
    public AudioClip healSE;
    public AudioClip dashSE;
    public AudioClip deadSE;
    public AudioClip hurtSE;

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
            Dash();
            DeadDetect();
        } else {
            // Maybe reset?
        }
    }

    void Init() {
        Firing = false;
        Dashing = false;
        Doing = false;
        transform.position = SpawnPoint;
        m_naviAgent.isStopped = false;
        DashEffect.SetActive(false);
        MedkitHealCD = true;
        FireEffect.SetActive(false);
        HealEffect.SetActive(false);
        DataManager.Instance.SetPlayerHP(MAXHP);
        DataManager.Instance.SetMAXHP(MAXHP);
        DataManager.Instance.PlayerDead(false);
        DataManager.Instance.SetDashCD(0);
        OriHP = MAXHP;
        ResetAnimDoing();
    }

    void LocateDestination() {
        if (Doing)
            return;
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Ground")) {
                    m_naviAgent.SetDestination(hit.point);
                }
            }
        }
        //audioPlayer.PlayOneShot(walkSE);
        Vector3 velocity = m_naviAgent.velocity;
        PlayerAnim.SetFloat("Speed", velocity.magnitude);
    }

    void FaceTarget() {
        if (m_naviAgent.velocity != Vector3.zero && !m_naviAgent.isStopped) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
        } else if (m_naviAgent.isStopped && FacingTarget != Vector3.zero) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * RotationSlerp * 2).eulerAngles.y, 0);
        }
    }

    public Vector3 PlayerPos() {
        return transform.position;
    }

    void SetGun() {
        if (rightGunBone.childCount > 0)
			Destroy(rightGunBone.GetChild(0).gameObject);
        if (rightGun != null) {
            GameObject newRightGun = (GameObject) Instantiate(rightGun);
            newRightGun.transform.parent = rightGunBone;
            newRightGun.transform.localPosition = Vector3.zero;
            newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }

    void Attack() {
        if (Input.GetKey(KeyCode.A) && !Firing) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 Target = Vector3.zero;
            if (Physics.Raycast(ray, out hit)) {
                Target = hit.point;
            }
            PlayerAnim.SetInteger("Doing", 1);
            audioPlayer.PlayOneShot(shootSE);
            Firing = true;
            FacingTarget = Target - transform.position;
            ToggleNavi();
            IEnumerator shoot = ShootBullet(Target);
            // IEnumerator shoot2 = ShootBullet2(Target);
            StartCoroutine(shoot);
            // StartCoroutine(shoot2);
            // Invoke("ToggleNavi", Shoot2WaitingTime + 0.11f);
            Invoke("ToggleNavi", ShootWaitingTime + 0.1f);
            Invoke("ResetAnimDoing", 0.05f);
            // Invoke("ResetFiring", Shoot2WaitingTime + ReloadSpeed);
            Invoke("ResetFiring", ReloadSpeed);
            Invoke("DisableFireEffect", FireEffectStop);
        }
    }

    void DisableFireEffect() {
        FireEffect.SetActive(false);
    }

    IEnumerator ShootBullet(Vector3 Target) {
        yield return new WaitForSeconds(ShootWaitingTime);
        Vector3 ShootDir = Target - ShooterPoint.position;
        ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        FireEffect.SetActive(true);
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
        Invoke("DisableFireEffect", FireEffectStop);
    }

    IEnumerator ShootBullet2(Vector3 Target) {
        yield return new WaitForSeconds(Shoot2WaitingTime);
        Vector3 ShootDir = Target - ShooterPoint.position;
        ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
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

    Vector3 GetMousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }

    void Dash() {
        if (Input.GetKey(KeyCode.Q) && !Dashing) {
            Dashing = true;
            DashEffect.SetActive(true);
            PlayerAnim.SetInteger("Doing", 2);
            ToggleNavi();
            Vector3 dir = GetMousePos() - transform.position;
            dir = new Vector3(dir.x, 0f, dir.z);
            dir = dir.normalized;
            FacingTarget = dir;
            Doing = true;
            IEnumerator dashMoving = DashMoving(dir);
            StartCoroutine(dashMoving);
            StartCoroutine(CalDashCD());
            Invoke("ResetDashing", DashCooldown);
            audioPlayer.PlayOneShot(dashSE);
        }
    }

    void ResetDoing() {
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
        ResetDoing();
        ToggleNavi();
        Invoke("DashEffectDisabled", 0.3f);
    }

    void DashEffectDisabled() {
        DashEffect.SetActive(false);
    }

    void ResetDashing() {
        Dashing = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Medkit")) {
            audioPlayer.PlayOneShot(healSE);
            Destroy(other.gameObject.transform.parent.gameObject);
            if (MedkitHealCD) {
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

    IEnumerator CalDashCD() {
        _DashCD = DashCooldown;
        while (_DashCD > 0) {
            _DashCD -= Time.deltaTime;
            DataManager.Instance.SetDashCD(_DashCD);
            yield return null;
        }
        _DashCD = 0;
        DataManager.Instance.SetDashCD(0);
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
                Doing = true;
                ToggleNavi();
                Invoke("LoadLoseScene", 4f);
            } else {
                if (CurHP < OriHP) {
                    audioPlayer.PlayOneShot(hurtSE);
                    Instantiate(DamagedEffect, transform.position, Quaternion.identity);
                    PlayerAnim.SetInteger("Doing", 4);
                    OriHP = CurHP;
                    Invoke("ResetAnimDoing", 0.2f);
                }
            }
        }
    }

    void LoadLoseScene() {
        SceneManager.LoadScene("Lose");
    }

    public void Reset() {
        Init();
    }

}
