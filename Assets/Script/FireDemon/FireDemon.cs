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
    public GameObject LeftSword;
    public GameObject ShockWaveCircle;
    public GameObject ShockWaveVertical;
    public GameObject BurnEffect;
    public GameObject FireBall;
    public GameObject Mouth;
    public GameObject BioFlame;
    public GameObject RevivingFire;
    public GameObject HumanCollider;

    public float RotationSlerp;
    public float DemonRotationSlerp;
    public float HumanMAXHP;
    public float DemonMAXHP;
    public float HP;
    public float HumanSpeed;
    public float DemonSpeed;
    public float HumanScale;
    public float DemonScale;

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
    public float OriSwordLen;
    public float SemiCircleSwordLen;
    public float SemiCircleStartCreating;
    public float SemiCircleSwordCreated;
    public float RX;
    public float RY;
    public float RZ;

    [Header("Demon")]
    public float ReviveDis;
    public float DemonChaseDis;
    public float ShockWaveBefore;
    public float ShockWaveOnFloor;
    public float ShockWaveStart;
    public float ShockWaveEnd;
    public float ShockWaveAnimWait;
    public float ShockWaveGrow;
    public float ShockWaveCircleRad;
    public float ShockWavePrepare;
    public float ShockWaveRange;
    public float ShockWaveDamage;
    public float CrushAttackBefore;
    public float CrushAttackDmgTrigger;
    public float CrushAttackRange;
    public float CrushAttackAng;
    public float CrushAttackDamage;
    public float CrushAttackAnimWait;
    public int MaxBurnStack;
    public float BurnStackDamage;
    public float ShootFireBallForce;
    public float FireBallBefore;
    public float FireBallShootFirst;
    public float FireBallShootSecond;
    public float FireBallShootThird;
    public float ShootEnd;
    public float FireBallShootRad;
    public float Falling1;
    public float Falling2;
    public float Falling3;
    public float FireBallAnimWait;
    public float FallFireBallForce;
    public float FlameBreatheBefore;
    public float StartFlaming;
    public float FinishFlaming;
    public float BurnFinish;
    public float FlameBreathAnimWait;
    public float FlameHitFreq;
    public float FlameIncBio;
    public float FlameRange;
    public float FlameAng;
    public float FlameTooClose;
    public int FlameStack;
    public float RushBefore;
    public float StartRush;
    public float RushEnd;
    public float RushAnimWait;
    public float RushSpeed;
    public float RushHitRange;
    public float RushDamage;
    public float MeleeAttackBefore;
    public float MeleeAttackHandUp;
    public float MeleeAttackStart;
    public float MeleeAttackDmgTrigger;
    public float MeleeAttackAnimWait;
    public float MeleeAttackRange;
    public float MeleeAttackAng;
    public float MeleeAttackDamage;

    [Header("Sound")]
    public AudioSource AS;
    public AudioClip Attack1Clip;
    public AudioClip Attack2Clip;
    public float Attack1ClipWait;
    public AudioClip KickClip;
    public AudioClip StabClip;
    public float StabSoundWait;
    public AudioClip SemiCircleSlashClip;
    public AudioClip SpeedUpClip;
    public AudioClip Combo1Clip;
    public AudioClip Combo2Clip;
    public AudioClip Combo3Clip;
    public AudioClip ShockWaveClip;
    public AudioClip FlameBreathClip;
    public AudioClip ShootFireBallClip;
    public AudioClip MeleeAttackClip;
    public AudioClip CrushAttackClip;
    public float CrushAttackSFXWait;

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
        DataManager.Instance.BossName = "Hephaestus";
        HP = HumanMAXHP;
        Phase = 1;
        Human.SetActive(true);
        StabSword.SetActive(false);
        SemiCircleRangeInd.SetActive(false);
        Demon.SetActive(false);
        Human_naviAgent = Human.GetComponent<NavMeshAgent>();
        Demon_naviAgent = Demon.GetComponent<NavMeshAgent>();
        HumanAnim = Human.GetComponent<Animator>();
        DemonAnim = Demon.GetComponent<Animator>();
        HumanAnim.SetBool("IsDead", false);
        LastSkill = -1;
        combo = false;
        DoNotShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DoNotShow)
            DataManager.Instance.ShowBossHP = true;
        else
            DataManager.Instance.ShowBossHP = false;
        
        if (Phase == 1)
            DataManager.Instance.BossMAXHP = HumanMAXHP;
        else
            DataManager.Instance.BossMAXHP = DemonMAXHP;
        DataManager.Instance.BossHP = HP;

        if (!IsDead)
            BurnerCheck();

        if (!DoNotTurn && !IsDead)
            FaceTarget();

        if (Phase == 1) {
            if (!Casting && !Freeze && !IsDead) {
                if (Vector3.Distance(DataManager.Instance.PlayerPos, Human.transform.position) >= ChaseDis) {
                    FarSkill();
                } else {
                    HumanAnim.SetBool("Walk", false);
                    CloseSkill();
                }
            }
        } else if (Phase == 2) {
            if (IsDead && !Reviving) {
                if (Dis() <= ReviveDis) {
                    StartCoroutine(ReviveAnim());
                }
            } else if (!IsDead) {
                if (!Casting && !Freeze) {
                    if (Vector3.Distance(DataManager.Instance.PlayerPos, Demon.transform.position) >= DemonChaseDis) {
                        DemonFarSkill();
                    } else {
                        DemonAnim.SetBool("Walk Forward", false);
                        DemonCloseSkill();
                    }
                }
            }
        }
    }

    void BurnerCheck() {
        if (DataManager.Instance.Burner) {
            DataManager.Instance.Burner = false;
            BurnStack++;
            CheckBurn();
        }
    }

    private bool Reviving;
    private bool DoNotShow;
    IEnumerator ReviveAnim() {
        Reviving = true;
        GameObject Rev = Instantiate(RevivingFire, Demon.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(4f);
        Demon.SetActive(true);
        BioFlame.SetActive(false);
        ShockWaveCircle.SetActive(false);
        ShockWaveVertical.SetActive(false);
        Human.SetActive(false);
        Demon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 Into = new Vector3(DemonScale, DemonScale, DemonScale);
        DemonAnim.SetTrigger("Jump");
        while (Demon.transform.localScale.x < DemonScale*0.98f) {
            Demon.transform.localScale = Vector3.Slerp(Demon.transform.localScale, Into, Time.deltaTime);
            yield return null;
        }
        Destroy(Rev);
        Demon.transform.localScale = Into;
        DataManager.Instance.BossName = "\"FIRE DEMON\" Hephaestus";
        DoNotShow = false;
        HP = 0;
        while (HP < DemonMAXHP) {
            HP += DemonMAXHP*0.0033f;
            yield return null;
        }
        HP = DemonMAXHP;
        yield return new WaitForSeconds(2f);
        IsDead = false;
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
        int SkillNum = -1;
        bool ch = false;
        while (!ch) {
            SkillNum = Random.Range(1, 101);
            if (combo) {
                ch = true;
                combo = false;
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
            if (!IsDead && Demon_naviAgent.enabled && Demon_naviAgent.velocity != Vector3.zero && !Demon_naviAgent.isStopped) {
                Demon.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Demon.transform.rotation, Quaternion.LookRotation(Demon_naviAgent.velocity - Vector3.zero), Time.deltaTime * DemonRotationSlerp).eulerAngles.y, 0);
            } else if (!IsDead && FacingTarget != Vector3.zero) {
                Demon.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Demon.transform.rotation, Quaternion.LookRotation(FacingTarget), Time.deltaTime * DemonRotationSlerp * 2).eulerAngles.y, 0);
            }
        } else {
            if (!IsDead && Human_naviAgent.velocity != Vector3.zero && !Human_naviAgent.isStopped && !Casting) {
                Human.transform.eulerAngles = new Vector3(0, Quaternion.Slerp(Human.transform.rotation, Quaternion.LookRotation(Human_naviAgent.velocity - Vector3.zero), Time.deltaTime * RotationSlerp).eulerAngles.y, 0);
            } else if (!IsDead && FacingTarget != Vector3.zero) {
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
        HumanAnim.SetFloat("Attack1Speed", 0.01f);
        yield return new WaitForSeconds(Attack1Slash);
        HumanAnim.SetFloat("Attack1Speed", 1f);
        yield return new WaitForSeconds(Attack1ClipWait);
        AS.PlayOneShot(Attack1Clip);
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
        AS.PlayOneShot(KickClip);
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
        Human_naviAgent.ResetPath();
        yield return new WaitForSeconds(SpeedUpBefore);
        HumanAnim.SetTrigger("SpeedUp");
        AS.PlayOneShot(SpeedUpClip);
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
        StartCoroutine(CoolDownCal(StabDmgTrigger+StabSwordStartCreate+StabSwordCreated+StabAnimWait+StabSoundWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(StabDmgTrigger+StabSwordStartCreate+StabSwordCreated+StabAnimWait+StabSoundWait, (returnVal1) => {
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
        AS.PlayOneShot(StabClip);
        yield return new WaitForSeconds(StabSoundWait);
        HumanAnim.SetTrigger("Stab");
        yield return new WaitForSeconds(StabDmgTrigger);
        if (Dis() <= StabRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= StabAng) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(StabDamage);
            DataManager.Instance.PlayerKnockDown = true;
        }
        yield return new WaitForSeconds(StabSwordDestroy);
        StabSword.SetActive(false);
        // yield return new WaitForSeconds(StabAnimWait);
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
        AS.PlayOneShot(Attack1Clip);
        if (Dis() <= OneSwordComboTooClose || (Dis() <= OneSwordComboRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= OneSwordComboAng)) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(OneSwordComboDamage1);
        }
        yield return new WaitForSeconds(OneSwordComboDmgTri2);
        AS.PlayOneShot(Attack2Clip);
        if (Dis() <= OneSwordComboTooClose || (Dis() <= OneSwordComboRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= OneSwordComboAng)) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(OneSwordComboDamage2);
        }
        yield return new WaitForSeconds(OneSwordComboDmgTri3);
        AS.PlayOneShot(Attack1Clip);
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
        Human_naviAgent.ResetPath();
        HumanAnim.SetTrigger("SwordCreate2");
        Vector3 Into = LeftSword.transform.localScale;
        Into.z = SemiCircleSwordLen;
        while (LeftSword.transform.localScale.z < SemiCircleSwordLen*0.95f) {
            LeftSword.transform.localScale = Vector3.Slerp(LeftSword.transform.localScale, Into, Time.deltaTime*3);
            yield return null;
        }
        DoNotTurn = true;
        yield return new WaitForSeconds(SemiCircleStartCreating);
        DoNotTurn = false;
        LeftSword.transform.localScale = Into;
        FacingTarget = PlayerPos() - Human.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(SemiCircleSwordCreated);
        HumanAnim.SetTrigger("SemiCircleSlash");
        AS.PlayOneShot(SemiCircleSlashClip, 1f);
        yield return new WaitForSeconds(SemiCircleSlashBefore);
        DoNotTurn = true;
        SemiCircleRangeInd.SetActive(true);
        HumanAnim.SetFloat("SemiCircleSlashSpeed", 0.005f);
        Quaternion OriRot = LeftSword.transform.localRotation;
        LeftSword.transform.localRotation = Quaternion.Euler(RX, RY, RZ);
        yield return new WaitForSeconds(SemiCircleStartSlash);
        HumanAnim.SetFloat("SemiCircleSlashSpeed", 1);
        if (Dis() <= SemiCircleSlashRange && Vector3.Dot(MeToPlayer(), Human.transform.forward) >= 0) {
            DataManager.Instance.KnockDownFrom = Human;
            DataManager.Instance.PlayerOnHit(SemiCircleSlashDamage);
            DataManager.Instance.PlayerKnockDown = true;
        }
        yield return new WaitForSeconds(SemiCircleSlashEnd);
        SemiCircleRangeInd.SetActive(false);
        Into.z = OriSwordLen;
        LeftSword.transform.localRotation = OriRot;
        while (LeftSword.transform.localScale.z > OriSwordLen*1.05f) {
            LeftSword.transform.localScale = Vector3.Slerp(LeftSword.transform.localScale, Into, Time.deltaTime*3);
            yield return null;
        }
        yield return new WaitForSeconds(SemiCircleSlashAnimWait);
        LeftSword.transform.localScale = Into;
    }
    
    // Check Death
    private bool IsDead;
    public void Damage(float damage) {
        if (IsDead)
            return;
        HP -= damage;
        if (HP <= 0) {
            IsDead = true;
            if (Phase == 1) {
                Human_naviAgent.ResetPath();
                HumanAnim.SetBool("IsDead", true);
                HumanAnim.SetTrigger("Dead");
                Demon.transform.position = Human.transform.position;
                HumanCollider.GetComponent<CapsuleCollider>().enabled = false;
                Invoke("DisableHealthBar", 2);
                Invoke("PhaseChange", 10);
            }
            else {
                Demon_naviAgent.ResetPath();
                DemonAnim.SetTrigger("Die");
                Invoke("DisableHealthBar", 2);
                // Invoke("PhaseChange", 10);
                // Defeated
            }
        }
    }

    void DisableHealthBar() {
        Human_naviAgent.enabled = false;
        DoNotShow = true;
    }

    void PhaseChange() {
        Phase = 2;
    }

    private bool Freeze;
    private float OriSpeed;
    public void Frozen() {
        if (IsDead) return;
        if (Freeze) {
            CancelInvoke("ResetFrozen");
        }
        if (Phase == 1)
            Human_naviAgent.speed = 0;
        else
            Demon_naviAgent.speed = 0;
        Freeze = true;
        HumanAnim.SetBool("Walk", false);
        Invoke("ResetFrozen", 2f);
    }

    void ResetFrozen() {
        Freeze = false;
        if (Phase == 1)
            Human_naviAgent.speed = HumanSpeed;
        else
            Demon_naviAgent.speed = DemonSpeed;
    }

    void DemonFarSkill() {
        if (WalkingCounter >= 3) {
            if (Dis() <= 15) {
                DemonAnim.SetBool("Walk Forward", false);
                float x = Random.Range(0, 3);
                WalkingCounter = 0;
                if (x < 1) {
                    LastSkill = 4;
                    ShootFireBall();
                } else if (x < 2) {
                    FlameBreathe();
                }
            }
        } else {
            WalkingCounter += Time.deltaTime;
            Demon_naviAgent.stoppingDistance = DemonChaseDis;
            Demon_naviAgent.SetDestination(DataManager.Instance.PlayerPos);
            DemonAnim.SetBool("Walk Forward", true);
        }
    }

    void DemonCloseSkill() {
        WalkingCounter = 0;
        int SkillNum = -1;
        bool ch = false;
        while (!ch) {
            SkillNum = Random.Range(1, 101);
            if (SkillNum <= 60) { // 60
                ch = true;
                SkillNum = 0;
            } else if (SkillNum <= 70 && LastSkill != 1) { // 10
                ch = true;
                SkillNum = 1;
            } else if (SkillNum <= 88 && LastSkill != 2) { // 18
                ch = true;
                SkillNum = 2;
            } else if (SkillNum <= 93 && LastSkill != 3) { // 5
                ch = true;
                SkillNum = 3;
            } else if (LastSkill != 4) { // 7
                ch = true;
                SkillNum = 4;
            }
        }
        LastSkill = SkillNum;
        if (SkillNum == 0)
            MeleeAttack();
        else if (SkillNum == 1) {
            ShockWave();
        }
        else if (SkillNum == 2) {
            CrushAttack();
        } else if (SkillNum == 3) 
            FlameBreathe();
        else if (SkillNum == 4) 
            ShootFireBall();
    }

    void ShockWave() {
        Casting = true;
        StartCoroutine(IShockWave());
        StartCoroutine(CoolDownCal(ShockWaveBefore+ShockWaveOnFloor+ShockWaveStart+ShockWaveEnd+ShockWavePrepare+ShockWaveAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(ShockWaveBefore+ShockWaveOnFloor+ShockWaveStart+ShockWaveEnd+ShockWavePrepare+ShockWaveAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IShockWave() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(ShockWaveBefore);
        DoNotTurn = true;
        DemonAnim.SetTrigger("Shockwave Attack");
        yield return new WaitForSeconds(ShockWaveOnFloor);
        DemonAnim.SetFloat("ShockWaveSpeed", 0.01f);
        ShockWaveCircle.transform.localScale = new Vector3(0, 0, 0);
        ShockWaveCircle.SetActive(true);
        ShockWaveVertical.SetActive(false);
        Vector3 Into = new Vector3(ShockWaveCircleRad,ShockWaveCircleRad,ShockWaveCircleRad);
        while (ShockWaveCircle.transform.localScale.x < ShockWaveCircleRad * 0.95f) {
            ShockWaveCircle.transform.localScale = Vector3.Slerp(ShockWaveCircle.transform.localScale, Into, Time.deltaTime*ShockWaveGrow);
            yield return null;
        }
        ShockWaveCircle.transform.localScale = Into;
        yield return new WaitForSeconds(ShockWavePrepare);
        DemonAnim.SetFloat("ShockWaveSpeed", 1);
        AS.PlayOneShot(ShockWaveClip);
        yield return new WaitForSeconds(ShockWaveStart);
        ShockWaveVertical.SetActive(true);
        if (Dis() <= ShockWaveRange) {
            DataManager.Instance.KnockDownFrom = Demon;
            DataManager.Instance.PlayerOnHit(ShockWaveDamage);
            DataManager.Instance.PlayerKnockDown = true;
        }
        yield return new WaitForSeconds(ShockWaveEnd);
        ShockWaveCircle.SetActive(false);
        ShockWaveVertical.SetActive(false);
    }

    void CrushAttack() {
        Casting = true;
        StartCoroutine(ICrushAttack());
        StartCoroutine(CoolDownCal(CrushAttackBefore+CrushAttackDmgTrigger+CrushAttackAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(CrushAttackBefore+CrushAttackDmgTrigger+CrushAttackAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    private int BurnStack;
    IEnumerator ICrushAttack() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(CrushAttackBefore);
        DoNotTurn = true;
        DemonAnim.SetTrigger("Crush Attack");
        yield return new WaitForSeconds(CrushAttackSFXWait);
        AS.PlayOneShot(CrushAttackClip);
        yield return new WaitForSeconds(CrushAttackDmgTrigger-CrushAttackSFXWait);
        if (Dis() <= CrushAttackRange && Vector3.Dot(MeToPlayer(), Demon.transform.forward) >= CrushAttackAng) {
            DataManager.Instance.KnockDownFrom = Demon;
            DataManager.Instance.PlayerOnHit(CrushAttackDamage);
            DataManager.Instance.PlayerKnockDown = true;
            BurnStack++;
            CheckBurn();
        }
    }

    void CheckBurn() {
        if (BurnStack >= MaxBurnStack) {
            BurnStack -= MaxBurnStack;
            GameObject BurnEffectPrefab = Instantiate(BurnEffect, DataManager.Instance.PlayerPos, Quaternion.identity);
            DataManager.Instance.PlayerOnHit(BurnStackDamage);
            Destroy(BurnEffectPrefab, 0.8f);
        }
    }

    void ShootFireBall() {
        Casting = true;
        StartCoroutine(IShootFireBall());
        StartCoroutine(CoolDownCal(FireBallBefore+FireBallShootFirst+FireBallShootSecond+FireBallShootThird+ShootEnd+Falling1+Falling2+Falling3+FireBallAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(FireBallBefore+FireBallShootFirst+FireBallShootSecond+FireBallShootThird+ShootEnd+Falling1+Falling2+Falling3+FireBallAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IShootFireBall() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        Demon_naviAgent.ResetPath();
        yield return new WaitForSeconds(FireBallBefore);
        DoNotTurn = true;
        DemonAnim.SetTrigger("Cast Spell");
        yield return new WaitForSeconds(FireBallShootFirst);
        AS.PlayOneShot(ShootFireBallClip);
        DemonAnim.SetFloat("CastSpellSpeed", 0.05f);
        GameObject FireBallPre1 = Instantiate(FireBall, Mouth.transform.position, Quaternion.identity);
        FireBallPre1.GetComponent<Rigidbody>().AddForce(0, ShootFireBallForce, 0);
        yield return new WaitForSeconds(FireBallShootSecond);
        AS.PlayOneShot(ShootFireBallClip);
        GameObject FireBallPre2 = Instantiate(FireBall, Mouth.transform.position, Quaternion.identity);
        FireBallPre2.GetComponent<Rigidbody>().AddForce(0, ShootFireBallForce, 0);
        yield return new WaitForSeconds(FireBallShootThird);
        AS.PlayOneShot(ShootFireBallClip);
        GameObject FireBallPre3 = Instantiate(FireBall, Mouth.transform.position, Quaternion.identity);
        FireBallPre3.GetComponent<Rigidbody>().AddForce(0, ShootFireBallForce, 0);
        yield return new WaitForSeconds(ShootEnd);
        DemonAnim.SetFloat("CastSpellSpeed", 1);
        
        yield return new WaitForSeconds(Falling1);
        float Rad1 = Random.Range(0, FireBallShootRad);
        float x1 = Random.Range(-Rad1, Rad1);
        float z1 = Mathf.Sqrt(Rad1*Rad1-x1*x1);
        float sign1 = Random.Range(-1, 1);
        if (sign1 < 0) z1 = -z1;
        Vector3 P = PlayerPos();
        FireBallPre1.transform.position = new Vector3(P.x + x1, FireBallPre1.transform.position.y, P.z + z1);
        FireBallPre1.GetComponent<Rigidbody>().AddForce(0, FallFireBallForce, 0);
        yield return new WaitForSeconds(Falling2);
        float Rad2 = Random.Range(0, FireBallShootRad);
        float x2 = Random.Range(-Rad2, Rad2);
        float z2 = Mathf.Sqrt(Rad2*Rad2-x2*x2);
        float sign2 = Random.Range(-1, 1);
        if (sign2 < 0) z2 = -z2;
        P = PlayerPos();
        FireBallPre2.transform.position = new Vector3(P.x + x2, FireBallPre2.transform.position.y, P.z + z2);
        FireBallPre2.GetComponent<Rigidbody>().AddForce(0, FallFireBallForce, 0);
        yield return new WaitForSeconds(Falling3);
        float Rad3 = Random.Range(0, FireBallShootRad);
        float x3 = Random.Range(-Rad3, Rad3);
        float z3 = Mathf.Sqrt(Rad3*Rad3-x3*x3);
        float sign3 = Random.Range(-1, 1);
        if (sign3 < 0) z3 = -z3;
        P = PlayerPos();
        FireBallPre3.transform.position = new Vector3(P.x + x3, FireBallPre3.transform.position.y, P.z + z3);
        FireBallPre3.GetComponent<Rigidbody>().AddForce(0, FallFireBallForce, 0);
    }

    void FlameBreathe() {
        Casting = true;
        StartCoroutine(IFlameBreathe());
        StartCoroutine(CoolDownCal(FlameBreatheBefore+StartFlaming+FinishFlaming+BurnFinish+FlameBreathAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(FlameBreatheBefore+StartFlaming+FinishFlaming+BurnFinish+FlameBreathAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IFlameBreathe() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        Demon_naviAgent.ResetPath();
        yield return new WaitForSeconds(FlameBreatheBefore);
        DoNotTurn = true;
        DemonAnim.SetFloat("FireBreathSpeed", 1);
        DemonAnim.SetTrigger("Fire Breath Attack");
        AS.PlayOneShot(FlameBreathClip);
        yield return new WaitForSeconds(StartFlaming);
        DemonAnim.SetFloat("FireBreathSpeed", 0.3f);
        BioFlame.SetActive(true);
        Coroutine X = StartCoroutine(CheckFlameHit());
        yield return new WaitForSeconds(FinishFlaming);
        StopCoroutine(X);
        DemonAnim.SetFloat("FireBreathSpeed", 1);
        yield return new WaitForSeconds(BurnFinish);
        BioFlame.SetActive(false);
    }

    IEnumerator CheckFlameHit() {
        int cnt = 0;
        while (true) {
            if (Dis() <= FlameTooClose || (Dis() <= FlameRange && Vector3.Dot(MeToPlayer(), Demon.transform.forward) >= FlameAng)) {
                DataManager.Instance.IncreaseBioVal(FlameIncBio);
                cnt++;
                if (cnt == FlameStack) {
                    BurnStack++;
                    CheckBurn();
                    cnt = 0;
                }
            }
            yield return new WaitForSeconds(FlameHitFreq);
        }
    }

    void DemonRush() {
        Casting = true;
        StartCoroutine(IRush());
        StartCoroutine(CoolDownCal(RushBefore+StartRush+RushEnd+RushAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(RushBefore+StartRush+RushEnd+RushAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IRush() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(RushBefore);
        DoNotTurn = true;
        yield return new WaitForSeconds(StartRush);
        DemonAnim.SetBool("Run Forward", true);
        Coroutine X = StartCoroutine(CheckRushHit());
        Demon_naviAgent.speed = RushSpeed;
        Demon_naviAgent.stoppingDistance = 1;
        float dur = 0;
        while (dur < RushEnd) {
            Demon_naviAgent.SetDestination(Demon.transform.position);
            dur += Time.deltaTime;
            yield return null;
        }
        Demon_naviAgent.ResetPath();
        Demon_naviAgent.speed = DemonSpeed;
        Demon_naviAgent.stoppingDistance = DemonChaseDis;
        DemonAnim.SetBool("Run Forward", false);
        StopCoroutine(X);
    }

    IEnumerator CheckRushHit() {
        bool ch = false;
        while (!ch) {
            if (Dis() <= RushHitRange) {
                DataManager.Instance.KnockDownFrom = Demon;
                DataManager.Instance.PlayerOnHit(RushDamage);
                DataManager.Instance.PlayerKnockDown = true;
                ch = true;
            }
            yield return null;
        }
    }

    void MeleeAttack() {
        Casting = true;
        StartCoroutine(IMeleeAttack());
        StartCoroutine(CoolDownCal(MeleeAttackBefore+MeleeAttackHandUp+MeleeAttackStart+MeleeAttackDmgTrigger+MeleeAttackAnimWait, (returnVal1) => {
            Casting = returnVal1;
        }));
        StartCoroutine(CoolDownCal(MeleeAttackBefore+MeleeAttackHandUp+MeleeAttackStart+MeleeAttackDmgTrigger+MeleeAttackAnimWait, (returnVal1) => {
            DoNotTurn = returnVal1;
        }));
    }

    IEnumerator IMeleeAttack() {
        FacingTarget = PlayerPos() - Demon.transform.position;
        FacingTarget.y = 0;
        yield return new WaitForSeconds(MeleeAttackBefore);
        DemonAnim.SetFloat("MeleeAttackSpeed", 1);
        float r = Random.Range(-1, 1);
        if (r < 0)
            DemonAnim.SetTrigger("Melee Attack 01");
        else
            DemonAnim.SetTrigger("Melee Attack 02");
        yield return new WaitForSeconds(MeleeAttackHandUp);
        DemonAnim.SetFloat("MeleeAttackSpeed", 0.01f);
        yield return new WaitForSeconds(MeleeAttackStart);
        AS.PlayOneShot(MeleeAttackClip);
        DemonAnim.SetFloat("MeleeAttackSpeed", 1);
        yield return new WaitForSeconds(MeleeAttackDmgTrigger);
        if (Dis() <= MeleeAttackRange && Vector3.Dot(MeToPlayer(), Demon.transform.forward) >= MeleeAttackAng) {
            DataManager.Instance.PlayerOnHit(MeleeAttackDamage);
            BurnStack++;
            CheckBurn();
        }
    }
}