using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireDemon : MonoBehaviour
{
    public GameObject Human;
    public GameObject Demon;
    public GameObject StabSword;
    public GameObject HumanRightHand;
    public GameObject SemiCircleRangeInd;

    public float RotationSlerp;
    public float DemonRotationSlerp;
    public float MAXHP;
    public float HP;
    public float HumanSpeed;
    public float DemonSpeed;

    [Header("Human")]
    public float ChaseDis;
    public float StoppingDis;
    public float HumanFarSlashDis;
    public float HumanAttack1CoolDown;
    public float HumanAttack1Before;
    public float Attack1AnimWait;
    public float Attack1SlashRange;
    public float Attack1SlashAng;
    public float Attack1Damage;
    public float Attack1DmgTrigger;
    public float Attack1HandUp;
    public float Attack1Slash;
    public float KickCoolDown;
    public float KickBefore;
    public float KickAnimWait;
    public float KickRange;
    public float KickAng;
    public float KickDamage;
    public float KickDmgTrigger;
    public float WalkCounterToSpeedUp;
    public float SpeedUpBefore;
    public float SpeedUpAnimWait;
    public float HumanBraverySpeed;
    public float SpeedUpCoolDown;
    public float SpeedUpRemain;
    public float StabSwordStartCreate;
    public float StabSwordCreated;
    public float StabAnimWait;
    public float StabCoolDown;
    public float StabDamage;
    public float StabDmgTrigger;
    public float StabRange;
    public float StabAng;
    public float StabSwordDestroy;
    public float OneSwordComboBefore;
    public float OneSwordComboDmgTri1;
    public float OneSwordComboDmgTri2;
    public float OneSwordComboDmgTri3;
    public float OneSwordComboAnimWait;
    public float OneSwordComboRange;
    public float OneSwordComboAng;
    public float OneSwordComboDamage1;
    public float OneSwordComboDamage2;
    public float OneSwordComboDamage3;
    public float OneSwordComboTooClose;
    public float SemiCircleSlashBefore;
    public float SemiCircleStartSlash;
    public float SemiCircleSlashEnd; 
    public float SemiCircleSlashAnimWait;
    public float SemiCirlceSlashCoolDown;
    public float SemiCircleSlashRange;
    public float SemiCircleSlashDamage;
    public float WalkingCounterToSemiCircle;

    private int Phase;
    private NavMeshAgent Human_naviAgent;
    private NavMeshAgent Demon_naviAgent;
    private Animator HumanAnim;
    private Animator DemonAnim;
    private Vector3 FacingTarget;
    private bool Casting;
    private bool DoNotTurn;

    void Start()
    {
        HP = MAXHP;
        Phase = 3;
        Human.SetActive(true);
        StabSword.SetActive(false);
        SemiCircleRangeInd.SetActive(false);
        Demon.SetActive(false);
        Human_naviAgent = Human.GetComponent<NavMeshAgent>();
        Demon_naviAgent = Demon.GetComponent<NavMeshAgent>();
        HumanAnim = Human.GetComponent<Animator>();
        DemonAnim = Demon.GetComponent<Animator>();
        LastSkill = -1;
        combo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DoNotTurn)
            FaceTarget();
        if (!SemiCircleSlashCD)
            HumanSemiCircleSlash();

        if (Phase == 1) {
            if (!Casting) {
                if (Vector3.Distance(DataManager.Instance.PlayerPos, Human.transform.position) >= ChaseDis) {
                    FarSkill();
                } else {
                    HumanAnim.SetBool("Walk", false);
                    CloseSkill();
                }
            }
        }
    }

    private float WalkingCounter;
    void FarSkill() {
        if (WalkingCounter >= WalkingCounterToSemiCircle && !SemiCircleSlashCD && Dis() <= HumanFarSlashDis) {
            WalkingCounter = 0;
            HumanAnim.SetBool("Walk", false);
            HumanSemiCircleSlash();
        } else if (WalkingCounter >= WalkCounterToSpeedUp && !HumanSpeedUpCD) {
            WalkingCounter = 0;
            HumanAnim.SetBool("Walk", false);
            HumanSpeedUp();
        } else {
            WalkingCounter += Time.deltaTime;
            Human_naviAgent.stoppingDistance = StoppingDis;
            Human_naviAgent.SetDestination(DataManager.Instance.PlayerPos);
            HumanAnim.SetBool("Walk", true);
        }
    }

    private bool combo;
    private int LastSkill;
    void CloseSkill() {
        WalkingCounter = 0;
        int SkillNum = 4;
        bool ch = true;
        while (!ch) {
            SkillNum = Random.Range(1, 101);
            if (combo) {
                ch = true;
                SkillNum = 0;
            }
            else if (SkillNum <= 45) { // 45
                ch = true;
                SkillNum = 0;
            } else if (SkillNum <= 65 && LastSkill != 1) { // 20
                ch = true;
                SkillNum = 1;
            } else if (SkillNum <= 70 && LastSkill != 2) { // 5
                ch = true;
                SkillNum = 2;
            } else if (SkillNum <= 85 && LastSkill != 3) { // 15
                ch = true;
                SkillNum = 3;
            } else if (LastSkill != 4) { // 15
                ch = true;
                SkillNum = 4;
            }
        }
        LastSkill = SkillNum;
        if (SkillNum == 0)
            HumanAttack1();
        else if (SkillNum == 1) {
            combo = true;
            HumanKick();
        }
        else if (SkillNum == 2) {
            combo = false;
            HumanKick();
        } else if (SkillNum == 3) 
            Stab();
        else if (SkillNum == 4) 
            OneSwordCombo();
    }

    float Dis() {
        if (Phase == 1)
            return Vector3.Distance(DataManager.Instance.PlayerPos, Human.transform.position);
        else
            return Vector3.Distance(DataManager.Instance.PlayerPos, Demon.transform.position);
    }

    Vector3 PlayerForward() {
        return DataManager.Instance.PlayerFacing;
    }

    Vector3 PlayerPos() {
        return DataManager.Instance.PlayerPos;
    }

    Vector3 MeToPlayer() {
        if (Phase == 1)
            return (PlayerPos() - Human.transform.position).normalized;
        else
            return (PlayerPos() - Demon.transform.position).normalized;
    }

    IEnumerator CoolDownCal(float coolDown, System.Action<bool> callback) {
        yield return new WaitForSeconds(coolDown);
        callback(false);
    }

    void FaceTarget() {
        if (Phase == 2) {
            if (Demon_naviAgent.enabled && Demon_naviAgent.velocity != Vector3.zero && !Demon_naviAgent.isStopped) {
                Demon.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Demon.transform.rotation, Quaternion.LookRotation(Demon_naviAgent.velocity - Vector3.zero), Time.deltaTime * DemonRotationSlerp).eulerAngles.y, 0);
            } else if (Demon_naviAgent.enabled && Demon_naviAgent.isStopped && FacingTarget != Vector3.zero) {
                Demon.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Demon.transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * DemonRotationSlerp * 2).eulerAngles.y, 0);
            }
        } else {
            if (Human_naviAgent.velocity != Vector3.zero && !Human_naviAgent.isStopped && !Casting) {
                Human.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Human.transform.rotation, Quaternion.LookRotation(Human_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
            } else if (FacingTarget != Vector3.zero) {
                Human.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Human.transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * RotationSlerp * 2).eulerAngles.y, 0);
            }
        }
    }

    void HumanAttack1() {
        Casting = true;
        StartCoroutine(IHumanAttack1());
        StartCoroutine(CoolDownCal(HumanAttack1Before+Attack1AnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(HumanAttack1Before+Attack1AnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IHumanAttack1() {
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(HumanAttack1Before);
        DoNotTurn = true;
        HumanAnim.SetTrigger("Attack1");
        yield return new WaitForSeconds(Attack1HandUp);
        HumanAnim.SetFloat("Attack1Speed", 0f);
        yield return new WaitForSeconds(Attack1Slash);
        HumanAnim.SetFloat("Attack1Speed", 1f);
        yield return new WaitForSeconds(Attack1DmgTrigger);
        if (Dis() <= Attack1SlashRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= Attack1SlashAng)
            DataManager.Instance.PlayerOnHit(Attack1Damage);
    }

    void HumanKick() {
        Casting = true;
        StartCoroutine(IKick());
        StartCoroutine(CoolDownCal(KickBefore+KickAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(KickBefore+KickAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IKick() {
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(KickBefore);
        DoNotTurn = true;
        HumanAnim.SetTrigger("Kick");
        yield return new WaitForSeconds(KickDmgTrigger);
        bool OnKick = false;
        if (Dis() <= KickRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= KickAng) {
            DataManager.Instance.PlayerOnHit(KickDamage);
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerKnockDown = true;
            OnKick = true;
        }
        if (combo)
            combo = OnKick;
    }

    private bool HumanSpeedUpCD;
    void HumanSpeedUp() {
        Casting = true;
        HumanSpeedUpCD = true;
        StartCoroutine(IHumanSpeedUp());
        StartCoroutine(CoolDownCal(SpeedUpBefore+SpeedUpCoolDown, (returnVal1) => {
            HumanSpeedUpCD = returnVal1;
        }));
        StartCoroutine(CoolDownCal(SpeedUpBefore+SpeedUpAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(SpeedUpBefore+SpeedUpAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    public bool IsSpeedUp;
    IEnumerator IHumanSpeedUp() {
        DoNotTurn = true;
        IsSpeedUp = true;
        yield return new WaitForSeconds(SpeedUpBefore);
        HumanAnim.SetTrigger("SpeedUp");
        Human_naviAgent.speed = HumanBraverySpeed;
        Invoke("ResetSpeedUp", SpeedUpRemain);
    }

    void ResetSpeedUp() {
        IsSpeedUp = false;
        Human_naviAgent.speed = HumanSpeed;
    }

    void Stab() {
        Casting = true;
        StartCoroutine(IStab());
        StartCoroutine(CoolDownCal(StabDmgTrigger+StabSwordStartCreate+StabSwordCreated+StabAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(StabDmgTrigger+StabSwordStartCreate+StabSwordCreated+StabAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IStab() {
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        HumanAnim.SetTrigger("CreateSword");
        yield return new WaitForSeconds(StabSwordStartCreate);
        DoNotTurn = true;
        StabSword.SetActive(true);
        yield return new WaitForSeconds(StabSwordCreated);
        HumanAnim.SetTrigger("Stab");
        yield return new WaitForSeconds(StabDmgTrigger);
        if (Dis() <= StabRange && Vector3.Dot(PlayerPos() - HumanRightHand.transform.position, Human.transform.forward) >= StabAng) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(StabDamage);
        }
        yield return new WaitForSeconds(StabSwordDestroy);
        StabSword.SetActive(false);
        yield return new WaitForSeconds(StabAnimWait);
    }

    void OneSwordCombo() {
        Casting = true;
        StartCoroutine(IOneSwordCombo());
        StartCoroutine(CoolDownCal(OneSwordComboDmgTri1+OneSwordComboDmgTri2+OneSwordComboDmgTri3+OneSwordComboAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(OneSwordComboDmgTri1+OneSwordComboDmgTri2+OneSwordComboDmgTri3+OneSwordComboAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IOneSwordCombo() {
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(OneSwordComboBefore);
        DoNotTurn = true;
        HumanAnim.SetTrigger("OneHandCombo");
        yield return new WaitForSeconds(OneSwordComboDmgTri1);
        if (Dis() <= OneSwordComboTooClose || (Dis() <= OneSwordComboRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= OneSwordComboAng)) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(OneSwordComboDamage1);
        }
        yield return new WaitForSeconds(OneSwordComboDmgTri2);
        if (Dis() <= OneSwordComboTooClose || (Dis() <= OneSwordComboRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= OneSwordComboAng)) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(OneSwordComboDamage2);
        }
        yield return new WaitForSeconds(OneSwordComboDmgTri3);
        if (Dis() <= OneSwordComboTooClose || (Dis() <= OneSwordComboRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= OneSwordComboAng)) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(OneSwordComboDamage3);
        }
    }

    private bool SemiCircleSlashCD;
    void HumanSemiCircleSlash() {
        Casting = true;
        SemiCircleSlashCD = true;
        StartCoroutine(ISemiCircleSlash());
        StartCoroutine(CoolDownCal(SemiCircleSlashBefore+SemiCircleStartSlash+SemiCircleSlashEnd+SemiCircleSlashAnimWait+SemiCirlceSlashCoolDown, (returnVal1) => {
            SemiCircleSlashCD = returnVal1;
        }));
        StartCoroutine(CoolDownCal(SemiCircleSlashBefore+SemiCircleStartSlash+SemiCircleSlashEnd+SemiCircleSlashAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(SemiCircleSlashBefore+SemiCircleStartSlash+SemiCircleSlashEnd+SemiCircleSlashAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator ISemiCircleSlash() {
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        HumanAnim.SetTrigger("SemiCircleSlash");
        yield return new WaitForSeconds(SemiCircleSlashBefore);
        DoNotTurn = true;
        SemiCircleRangeInd.SetActive(true);
        HumanAnim.SetFloat("SemiCircleSlashSpeed", 0.01f);
        yield return new WaitForSeconds(SemiCircleStartSlash);
        HumanAnim.SetFloat("SemiCircleSlashSpeed", 1);
        if (Dis() <= SemiCircleSlashRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= 0) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(SemiCircleSlashDamage);
            DataManager.Instance.PlayerKnockDown = true;
        }
        yield return new WaitForSeconds(SemiCircleSlashEnd);
        SemiCircleRangeInd.SetActive(false);
    }
    
}
