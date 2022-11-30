using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploBugB : MonoBehaviour
{
    [Header("Settings")]
    public float MaximumChaseDis;
    public float ExistTime;
    public float Damage;
    public float BiteDistance;
    public float BiteCoolDown;

    private GameObject CurClosetEnemy;
    private UnityEngine.AI.NavMeshAgent m_naviAgent;
    private Animator Anim;

    void Start()
    {
        CurClosetEnemy = FindClosetEnemy();
        m_naviAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_naviAgent.isStopped = false;
        Anim = GetComponent<Animator>();
        StartCoroutine(DestroyMe());
    }

    void Update()
    {
        if (!die) {
            FlyOrAttack();
            FacingTarget();
        }
    }

    GameObject FindClosetEnemy() {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float ShortestDis = MaximumChaseDis;
        GameObject ClosetEnemy = null;
        foreach (var obj in Enemies) {
            Vector3 a = obj.transform.root.position;
            Vector3 b = transform.position;
            a.y = 0;
            b.y = 0;
            float dis = Vector3.Distance(a, b);
            if (dis == ShortestDis && ClosetEnemy == null) {
                ClosetEnemy = obj.transform.root.gameObject;
            } else if (dis < ShortestDis) {
                ClosetEnemy = obj.transform.root.gameObject;
                ShortestDis = dis;
            }
        }
        return ClosetEnemy;
    }

    private bool BiteCD;
    void FlyOrAttack() {
        if (CurClosetEnemy == null) {
            m_naviAgent.ResetPath();
            CurClosetEnemy = FindClosetEnemy();
        }
        else {
            Vector3 a = CurClosetEnemy.transform.position;
            Vector3 b = transform.position;
            a.y = 0;
            b.y = 0;
            float dis = Vector3.Distance(a, b);
            if (dis <= BiteDistance) {
                if (!BiteCD) {
                    ToggleNavi();
                    Anim.SetBool("Fly Forward", false);
                    Anim.SetTrigger("Bite Attack");
                    DataManager.Instance.takedamage(CurClosetEnemy.transform, Damage);
                    BiteCD = true;
                    Invoke("ResetBiteCD", BiteCoolDown);
                }
            } else {
                if (m_naviAgent.isStopped)
                    ToggleNavi();
                Anim.SetBool("Fly Forward", true);
                m_naviAgent.SetDestination(CurClosetEnemy.transform.position);
            }
        }
    }

    void ResetBiteCD() {
        BiteCD = false;
    }

    void FacingTarget() {
        if (CurClosetEnemy == null)
            return;
        Vector3 FaceTarget = CurClosetEnemy.transform.position - transform.position;
        FaceTarget.y = 0;
        transform.eulerAngles = transform.eulerAngles = new Vector3(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FaceTarget), Time.deltaTime * 7 * 2).eulerAngles.y, 0);
    }

    void ToggleNavi() {
        if (m_naviAgent.isStopped == true) {
            m_naviAgent.isStopped = false;
        } else {
            m_naviAgent.ResetPath();
            m_naviAgent.isStopped = true;
        }
    }

    private bool die;
    IEnumerator DestroyMe() {
        yield return new WaitForSeconds(ExistTime);
        die = true;
        Anim.SetBool("Fly Forward", false);
        Anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
