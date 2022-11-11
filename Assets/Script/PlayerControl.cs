using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Camera PlayerCamera;
    public Transform rightGunBone;
    public GameObject rightGun;
    public GameObject Bullet;
    public Transform ShooterPoint;
    public GameObject Gam;
    public GameObject DashEffect;

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

    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private RaycastHit hit;
    private Animator PlayerAnim;
    private Rigidbody PlayerRb;
    private bool Firing;
    private GameObject BulletPrefab;
    private Vector3 FacingTarget;
    private float _HP;
    private GameController gam;
    private bool Dashing;
    private bool Doing;
    private bool MedkitHealCD;

    // Start is called before the first frame update
    void Start()
    {
        gam = Gam.GetComponent<GameController>();
        m_naviAgent = GetComponent<NavMeshAgent>();
        PlayerRb = GetComponent<Rigidbody>();
        PlayerAnim = GetComponent<Animator>();
        SetGun();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        LocateDestination();
        FaceTarget();
        Dash();
    }

    void Init() {
        Firing = false;
        _HP = gam.PlayerInitHP;
        Dashing = false;
        Doing = false;
        transform.position = SpawnPoint;
        DashEffect.SetActive(false);
        MedkitHealCD = true;
    }

    void LocateDestination() {
        if (Doing)
            return;
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Ground")) {
                    m_naviAgent.SetDestination(hit.point);
                } else if (hit.collider.CompareTag("Enemy")) {
                    Attack(hit.collider.transform.position);
                }
            }
        }

        Vector3 velocity = m_naviAgent.velocity;
        PlayerAnim.SetFloat("Speed", velocity.magnitude);
    }

    void FaceTarget() {
        if (m_naviAgent.velocity != Vector3.zero && !m_naviAgent.isStopped) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
        } else if (m_naviAgent.isStopped) {
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

    void Attack(Vector3 Target) {
        if (!Firing) {
            PlayerAnim.SetBool("Fire", true);
            if (m_naviAgent.velocity.magnitude > 0)
                PlayerAnim.SetInteger("NextStateAfterFire", 0);
            else
                PlayerAnim.SetInteger("NextStateAfterFire", 1);
            Firing = true;
            FacingTarget = Target - transform.position;
            ToggleNavi();
            IEnumerator shoot = ShootBullet(Target);
            // IEnumerator shoot2 = ShootBullet2(Target);
            StartCoroutine(shoot);
            // StartCoroutine(shoot2);
            // Invoke("ToggleNavi", Shoot2WaitingTime + 0.11f);
            Invoke("ToggleNavi", ShootWaitingTime + 0.1f);
            Invoke("ResetAnimFire", 0.05f);
            // Invoke("ResetFiring", Shoot2WaitingTime + ReloadSpeed);
            Invoke("ResetFiring", ReloadSpeed);
        }
    }

    IEnumerator ShootBullet(Vector3 Target) {
        yield return new WaitForSeconds(ShootWaitingTime);
        Vector3 ShootDir = Target - ShooterPoint.position;
        ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
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

    void ResetAnimFire() {
        PlayerAnim.SetBool("Fire", false);
    }

    void ResetFiring() {
        Firing = false;
    }

    public void OnHit(float damage) {
        _HP -= damage;
    }

    public void SetPlayerHP(float hp) {
        _HP = hp;
    }

    public float GetHP() {
        return _HP;
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
            PlayerAnim.SetBool("Dash", true);
            ToggleNavi();
            Vector3 dir = GetMousePos() - transform.position;
            dir = new Vector3(dir.x, 0f, dir.z);
            dir = dir.normalized;
            FacingTarget = dir;
            Doing = true;
            IEnumerator dashMoving = DashMoving(dir);
            StartCoroutine(dashMoving);
            Invoke("ResetDashing", DashCooldown);
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
        ResetAnimDash();
        ResetDoing();
        ToggleNavi();
        DashEffect.SetActive(false);
    }

    void ResetAnimDash() {
        PlayerAnim.SetBool("Dash", false);
    }

    void ResetDashing() {
        Dashing = false;
    }

    public void Heal(float HealHP) {
        _HP += HealHP;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Medkit")) {
            Destroy(other.gameObject.transform.parent.gameObject);
            if (MedkitHealCD) {
                MedkitHealCD = false;
                Heal(MedkitHealHP);
                Invoke("ResetMedkitHealCD", 0.2f);
            }
        }
    }

    void ResetMedkitHealCD() {
        MedkitHealCD = true;
    }

}
