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

    [Header("Settings")]
    public Vector3 SpawnPoint;
    public float RotationSlerp;
    public float ReloadSpeed;
    public float ShootForce;
    public float ShootWaitingTime;
    public float Shoot2WaitingTime;

    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private RaycastHit hit;
    private Animator PlayerAnim;
    private Rigidbody PlayerRb;
    private bool Firing;
    private bool ResetRotation;
    private GameObject BulletPrefab;
    private Vector3 ShootingTarget;
    private float _HP;
    private GameController gam;

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
    }

    void Init() {
        transform.position = SpawnPoint;
        Firing = false;
        ResetRotation = true;
        _HP = gam.PlayerInitHP;
    }

    void LocateDestination() {
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
        if (m_naviAgent.velocity != Vector3.zero && ResetRotation) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
        } else if (!ResetRotation) {
            transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ShootingTarget - transform.position), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
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
            Firing = true;
            ResetRotation = false;
            ShootingTarget = Target;
            IEnumerator shoot = ShootBullet(Target);
            IEnumerator shoot2 = ShootBullet2(Target);
            StartCoroutine(shoot);
            StartCoroutine(shoot2);
            m_naviAgent.isStopped = true;
            Invoke("ResetNaviAnim", Shoot2WaitingTime + 0.11f);
            Invoke("ResetFiring", Shoot2WaitingTime + ReloadSpeed);
        }
    }

    IEnumerator ShootBullet(Vector3 Target) {
        yield return new WaitForSeconds(ShootWaitingTime);
        Vector3 ShootDir = Target - ShooterPoint.position;
        // ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
    }

    IEnumerator ShootBullet2(Vector3 Target) {
        yield return new WaitForSeconds(Shoot2WaitingTime);
        Vector3 ShootDir = Target - ShooterPoint.position;
        // ShootDir = new Vector3(ShootDir.x, 0f, ShootDir.z).normalized;
        BulletPrefab = Instantiate(Bullet, ShooterPoint.position, Quaternion.identity);
        BulletPrefab.GetComponent<Rigidbody>().AddForce(ShootDir * ShootForce);
    }
    
    void ResetNaviAnim() {
        m_naviAgent.isStopped = false;
        PlayerAnim.SetBool("Fire", false);
        ResetRotation = true;
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
}
