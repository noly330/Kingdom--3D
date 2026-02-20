using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : CombatControllerBase
{

    public ComboListSO normalComboList;
    [SerializeField] private ComboListSO boxingComboList;
    [SerializeField] private ComboListSO fallComboList;
    private MoveController moveController;
    private PlayerInput playerInput;
    protected override void Awake()
    {
        base.Awake();
        moveController = GetComponent<MoveController>();
        playerInput = GetComponent<PlayerInput>();
    }

    protected override void Start()
    {
        base.Start();
        currentComboList = normalComboList;
    }
    protected override void Update()
    {
        base.Update();

        CheckInput();

        CheckState();
    }

    private void CheckInput()
    {
        if (playerInput.actions["Fire1"].triggered && canExecuteCombo)
        {
            //切换普攻，下落攻击，技能等
            if (moveController.isGround)
            {
                if (currentComboList.TryGetAttackType != AttackType.Normal)
                {
                    nextComboIndex = 0;
                    currentComboList = normalComboList;
                }

            }
            else
            {
                nextComboIndex = 0;
                currentComboList = fallComboList;
            }
            ExecuteCombo();
        }
    }
    public void UpdateNormalComboList(ComboListSO comboList)
    {
        if(comboList == null)
        {
            normalComboList = boxingComboList;
            currentComboList = boxingComboList;
            return ;
        }
        normalComboList = comboList;
        currentComboList = comboList;
    }

    private void CheckState()
    {
        if(currentCharacter.currentHealth <= 0f && !currentCharacter.isDead)
        {
            animator.SetBool("IsDead",true);
            currentCharacter.isDead = true;
            GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        }
    }

    protected override void CharacterCombatBeHit(ComboInteractionConfig interactionConfig, CharacterBase attacker)
    {
        base.CharacterCombatBeHit(interactionConfig, attacker);
        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        animator.Play(interactionConfig.hitName, 0, 0);
        //生成受击特效
        var fxObj = hitFXList[(int)interactionConfig.attackForce].TryGetOneFXObj();
        if(fxObj != null)
            ToolManager.instance.PlayOneFX(fxObj, hitPoints[0].position, Vector3.zero, new Vector3(1, 1, 1));
        //生成音效
    }

}
