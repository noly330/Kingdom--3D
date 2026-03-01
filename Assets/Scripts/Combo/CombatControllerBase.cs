using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class CombatControllerBase : MonoBehaviour
{
    public LayerMask targetMask;
    [Header("自动获取无需拖拽")]
    [SerializeField] protected ComboListSO currentComboList;
    public Animator animator;
    public bool canExecuteCombo;
    protected CharacterBase currentCharacter;
    private int currentComboIndex;
    protected int nextComboIndex;
    private Coroutine stopComboCoroutine;  //这次一个协程，为了防止多个协程同时触发
    private RunningEventIndex runningEventIndex;
    //runningEventIndex是 “运行时事件索引”，用来保证每个战斗事件只执行一次（比如一次攻击检测只触发一次，不会每帧重复检测）
    private CharacterController characterController;
    public CharacterBase M_currentCharacter => currentCharacter;

    protected virtual void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        canExecuteCombo = true;
        runningEventIndex = new RunningEventIndex();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        RunEvent();
    }

    /// <summary>
    /// 处理普通攻击的伤害传递，特效和音效
    /// </summary>
    private void RunEvent()
    {
        if (currentComboList == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(currentComboList.TryGetComboName(currentComboIndex))
        || animator.IsInTransition(0)) return;

        //攻击检测
        AttackDetectionConfig attackDetectionConfig =
        currentComboList.TryGetAttackDetectionConfig(currentComboIndex, runningEventIndex.attackDetectionIndex);

        if (attackDetectionConfig != null)
        {
            //攻击位移
            ExecuteMoveOffset(currentComboList.TryGetSelfMoveOffsetConfig(currentComboIndex,
            runningEventIndex.attackDetectionIndex), transform);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackDetectionConfig.startTime)
            {
                //执行攻击检测
                Vector3 boxPosition = transform.forward * attackDetectionConfig.position.z
                 + transform.up * attackDetectionConfig.position.y + transform.right * attackDetectionConfig.position.x;

                Collider[] targetList = Physics.OverlapBox(transform.position + boxPosition, attackDetectionConfig.scale, quaternion.identity, targetMask);
                foreach (var target in targetList)
                {
                    //攻击位移

                    //攻击敌人
                    target.GetComponent<CombatControllerBase>().CharacterCombatBeHit(
                        currentComboList.TryGetComboInteractionConfig(currentComboIndex, runningEventIndex.attackDetectionIndex), currentCharacter
                        );
                }
                //执行一次事件后
                runningEventIndex.attackDetectionIndex++;
            }
        }

        //生成特效
        FXConfig FX_Config = currentComboList.TryGetFXConfig(currentComboIndex, runningEventIndex.FXIndex);
        if (FX_Config != null)
        {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > FX_Config.startTime)
            {
                Vector3 fxPosition = transform.forward * FX_Config.position.z
                 + transform.up * FX_Config.position.y + transform.right * FX_Config.position.x;
                ToolManager.instance.PlayOneFX(FX_Config.FXObject, fxPosition
                + transform.position, transform.eulerAngles + FX_Config.rotation, FX_Config.scale);

                runningEventIndex.FXIndex++;

            }
        }

        //生成音效
        SFXConfig SFX_Config = currentComboList.TryGetSFXConfig(currentComboIndex, runningEventIndex.SFXIndex);
        if (SFX_Config != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > SFX_Config.startTime)
            {
                AudioManager.instance.PlaySFX(SFX_Config.audioClip, SFX_Config.volume);
                runningEventIndex.SFXIndex++;
            }
        }
    }

    #region 攻击
    public void ExecuteCombo()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) return;
        FindTarget();
        LookTarget();

        runningEventIndex.Reset();
        currentComboIndex = nextComboIndex;
        animator.CrossFadeInFixedTime(currentComboList.TryGetComboName(currentComboIndex), 0.155f, 0, 0);
        //Debug.Log("触发" + currentComboList.TryGetComboName(currentComboIndex));
        UpdateComboIndex();

        StartCoroutine(IE_ExecuteComboCold(currentComboList.TryGetColdTime(currentComboIndex)));
        if (stopComboCoroutine != null)
        {
            StopCoroutine(stopComboCoroutine);
        }

        stopComboCoroutine = StartCoroutine(IE_ResetComboIndex(currentComboList.TryGetColdTime(currentComboIndex)));
    }

    IEnumerator IE_ExecuteComboCold(float coldTime)
    {
        canExecuteCombo = false;
        while (coldTime > 0)
        {
            yield return null;
            coldTime -= Time.deltaTime;
        }
        canExecuteCombo = true;
    }
    IEnumerator IE_ResetComboIndex(float coldTime)
    {
        float time = 2.5f * coldTime;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        nextComboIndex = 0;
    }


    private void UpdateComboIndex()
    {
        nextComboIndex++;
        if (nextComboIndex >= currentComboList.TryGetComboListCount())
        {
            nextComboIndex = 0;
        }
    }

    #endregion

    #region 受击

    [Header("受击数据")]
    [SerializeField] protected HitFXConfig[] hitFXList;
    [SerializeField] protected Transform[] hitPoints;

    protected virtual void CharacterCombatBeHit(ComboInteractionConfig interactionConfig, CharacterBase attacker)
    {
        if (interactionConfig == null) return;
        if(currentCharacter.isInvulnerable) return;

        //传递伤害
        currentCharacter.OnBeHit(attacker.TryGetDamage(interactionConfig));

        //生成受击特效
        var fxObj = hitFXList[0].TryGetOneFXObj();
        if (fxObj != null)
            ToolManager.instance.PlayOneFX(fxObj, hitPoints[0].position, Vector3.zero, new Vector3(1, 1, 1));

        //判断攻击力度是否大于自身的韧性
        if(interactionConfig.attackForce < currentCharacter.poise)
            return;
        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        animator.Play(interactionConfig.hitName, 0, 0);
        //生成音效
    }

    #endregion

    #region 追踪敌人

    [Header("索敌设置")]
    private Transform currentTarget;
    public Vector3 checkSize = new Vector3(3, 3, 3);  //检测范围大小

    private void FindTarget()
    {
        //执行检测，获取所有目标
        Collider[] targetList = Physics.OverlapBox(transform.position, checkSize, Quaternion.identity, targetMask);
        if (targetList.Length == 0)
        {
            currentTarget = null;
            return;
        }

        //找最近目标
        float distanceMin = float.MaxValue;
        foreach (var target in targetList)
        {
            float dis = Vector3.Distance(target.transform.position, transform.position);
            if (dis < distanceMin)
            {
                distanceMin = dis;
                currentTarget = target.transform;
            }
        }
    }
    private void LookTarget()
    {
        if (currentTarget == null) return;
        Vector3 dir = currentTarget.position - transform.position;
        dir.y = 0;
        transform.forward = dir.normalized;
    }

    private Coroutine executeMoveOffsetCoroutine;
    protected void ExecuteMoveOffset(MoveOffsetConfig moveOffsetConfig, Transform user)
    {
        if (moveOffsetConfig == null) return;

        if (executeMoveOffsetCoroutine != null)
            StopCoroutine(executeMoveOffsetCoroutine);

        Vector3 dir = new Vector3(0, 0, 0);

        switch (moveOffsetConfig.moveOffsetDirection)
        {
            case MoveOffsetDirection.Forward:
                dir = user.forward;
                break;
            case MoveOffsetDirection.Up:
                dir = user.up;
                break;
        }

        executeMoveOffsetCoroutine = StartCoroutine(IE_ExecuteMoveOffset(moveOffsetConfig, dir));

    }

    IEnumerator IE_ExecuteMoveOffset(MoveOffsetConfig moveOffsetConfig, Vector3 dir)
    {
        //Debug.Log("位移" + moveOffsetConfig.moveOffsetDirection);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < moveOffsetConfig.duration)
        {
            yield return null;
            float value = moveOffsetConfig.animationCurve.Evaluate(animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * moveOffsetConfig.scale;
            characterController.Move(dir * value * Time.deltaTime);
        }
        executeMoveOffsetCoroutine = null;
    }

    #endregion

}

public class RunningEventIndex
{
    public int attackDetectionIndex = 0;
    public int FXIndex = 0;
    public int SFXIndex = 0;
    public int AttackFeedbackIndex = 0;
    public void Reset()
    {
        attackDetectionIndex = 0;
        FXIndex = 0;
        SFXIndex = 0;
        AttackFeedbackIndex = 0;
    }
}