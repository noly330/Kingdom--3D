using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachineBase : MonoBehaviour
{
    private CombatControllerBase _combatController;
    
    public Dictionary<CombatStateType, ICombatState> states = new Dictionary<CombatStateType, ICombatState>();

    [SerializeField] private CombatStateType _currentStateType = CombatStateType.None;
    public CombatStateType GetCurrentStateType() => _currentStateType;
    [SerializeField] private CombatStateType _defaultStateType = CombatStateType.None;

    protected virtual void Awake()
    {
        _combatController = GetComponent<CombatControllerBase>();
    }

    protected virtual void Start()
    {
        InitStates();
        
    }

    protected virtual void Update()
    {
        states[_currentStateType].OnUpdate();
    }

    protected virtual void InitStates()
    {
        states.Add(CombatStateType.None, new NoneState());
        states.Add(CombatStateType.NormalAttack, new NormalAttackState(_combatController, this));
        states.Add(CombatStateType.Hit, new HitState(_combatController, this));
    }

    #region 状态转换方法
    public void TryTransitionTO(CombatStateType newType)
    {
        //Debug.Log($"尝试从{_currentStateType}过渡到{newType}");
        if (!states.ContainsKey(newType))
        {
            Debug.LogError($"CombatStateMachine: TryTransitionTO({newType}) but {newType} is not in states");
            return;
        }

        if (_currentStateType == newType)
        {
            states[newType].OnEnterAgain();
            return;
        }

        if (GetStatePriority(_currentStateType) >= GetStatePriority(newType))
            return;

        TransitionTo(newType);
    }
    public void ForceTransitionTo(CombatStateType newType)
    {
        if (!states.ContainsKey(newType))
            return;

        if (_currentStateType == newType)
        {
            states[newType].OnEnterAgain();
            return;
        }

        TransitionTo(newType);
    }
    public void ReturnToDefaultState()
    {
        if (!states.ContainsKey(_defaultStateType))
        {
            Debug.LogError($"CombatStateMachine: default state {_defaultStateType} is not in states");
            return;
        }

        if (_currentStateType == _defaultStateType)
            return;

        TransitionTo(_defaultStateType);
    }

    private void TransitionTo(CombatStateType newType)
    {
        states[_currentStateType].OnExit();
        _currentStateType = newType;
        states[newType].OnEnter();
    }
    #endregion

    #region 其他辅助方法
    private float GetStatePriority(CombatStateType stateType)
    {
        switch (stateType)
        {
            case CombatStateType.None:
                return 0f;
            case CombatStateType.NormalAttack:
                return 10f;
            case CombatStateType.Skill:
                return 20f;
            case CombatStateType.LinkSkill:
                return 20f;
            case CombatStateType.Slide:
                return 40f;
            case CombatStateType.Avoid:
                return 50f;
            case CombatStateType.Hit:
                return 60f;
            case CombatStateType.UltimateSkill:
                return 80f;
            case CombatStateType.Dead:
                return 100f;
            default:
                return 0f;
        }
    }
    #endregion
}

public enum CombatStateType
{
    None,
    NormalAttack,
    Skill,
    LinkSkill,
    UltimateSkill,
    Slide,
    Avoid,
    Hit,
    Dead
}
