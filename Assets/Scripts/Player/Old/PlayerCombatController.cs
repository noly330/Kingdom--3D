using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : CombatControllerBase
{

    [Header("玩家战斗招式")]
    public ComboListSO normalComboList;
    [SerializeField] private ComboListSO boxingComboList;
    [SerializeField] private ComboListSO fallComboList;
    [SerializeField] private ComboListSO[] skillComboList;
    private PlayerMovementControl moveController;
    private PlayerInput playerInput;

    [Header("完美闪避相关")]
    public AudioClip perfectAvoidClip;
    public float perfectAvoidDuration = 0.8f;  // 完美闪避总时间
    public float meshRefreshRate = 0.2f;  // 残影生成间隔
    public float tScale = 0.4f;
    private float meshDestoryDelay = 1f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Material trailMaterial;


    private float hitStunDuration = 0.3f;  //受击僵直
    protected override void Awake()
    {
        base.Awake();
        moveController = GetComponent<PlayerMovementControl>();
        playerInput = GetComponent<PlayerInput>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
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

    }

    private void CheckInput()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
            return;

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
        if (playerInput.actions["Skill1"].triggered && canExecuteCombo)
        {
            if (currentCharacter.currentEnergy < skillComboList[0].energyCost)
                return;
            currentCharacter.currentEnergy -= skillComboList[0].energyCost;
            Debug.Log("触发技能1");
            nextComboIndex = 0;
            currentComboList = skillComboList[0];

            StartCoroutine(IE_ChangePoise(ForceLevel.Mid));
            ExecuteCombo();
        }
    }
    public void UpdateNormalComboList(ComboListSO comboList)
    {
        if (comboList == null)
        {
            normalComboList = boxingComboList;
            currentComboList = boxingComboList;
            return;
        }
        normalComboList = comboList;
        currentComboList = comboList;
    }

    public void TransitionToDeadState()
    {
        Debug.Log("触发死亡状态");
        playerInput.actions.FindActionMap("Player").Disable();
    }

    private Coroutine perfectAvoidCoroutine;
    protected override void CharacterCombatBeHit(ComboInteractionConfig interactionConfig, CharacterBase attacker)
    {
        //完美闪避
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Slide"))
        {
            if (perfectAvoidCoroutine == null)  //确保完美闪避不连续触发
            {
                currentCharacter.invulnerableTime += 0.15f;
                currentCharacter.currentEnergy += 8f;
                //播放完美闪避音效
                AudioManager.instance.PlaySFX(perfectAvoidClip, 0.8f);
                perfectAvoidCoroutine = StartCoroutine(IE_PerfectAvoid());
            }
            return;
        }

        base.CharacterCombatBeHit(interactionConfig, attacker);

        //受击僵直（防止玩家用闪避跳跃之类的取消受击后腰)
        if (hitStunCoroutine != null)
            StopCoroutine(hitStunCoroutine);
        hitStunCoroutine = StartCoroutine(IE_HitStun());
    }

    #region 受击僵直
    private Coroutine hitStunCoroutine;
    private IEnumerator IE_HitStun()
    {
        float duration = hitStunDuration;
        playerInput.actions.FindActionMap("Player").Disable();
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        playerInput.actions.FindActionMap("Player").Enable();
        hitStunCoroutine = null;
    }


    #endregion

    #region 完美闪避
    float meshRefreshTimer = 0f;

    IEnumerator IE_PerfectAvoid()
    {
        float duration = perfectAvoidDuration;
        meshRefreshTimer = 0f;

        if (skinnedMeshRenderers == null)
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        Time.timeScale = tScale;
        while (duration > 0)
        {
            duration -= Time.unscaledDeltaTime;
            meshRefreshTimer -= Time.unscaledDeltaTime;
            if (meshRefreshTimer < 0)
            {
                CreateAfterimage();
                meshRefreshTimer = meshRefreshRate;
            }
            yield return null;
        }

        Time.timeScale = 1f;
        perfectAvoidCoroutine = null;
    }
    private void CreateAfterimage()
    {
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            GameObject obj = new GameObject();

            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = -90;
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rot));
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            MeshFilter mf = obj.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            skinnedMeshRenderers[i].BakeMesh(mesh);
            mf.mesh = mesh;
            mr.material = skinnedMeshRenderers[i].material;

            Destroy(obj, meshDestoryDelay);
        }

    }
    #endregion


    #region  韧性切换

    private IEnumerator IE_ChangePoise(ForceLevel newPoise)
    {
        currentCharacter.poise = newPoise;
        //float time = animator.GetCurrentAnimatorStateInfo(0).length * 0.4f / animator.speed;
        float time = 1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        currentCharacter.poise = ForceLevel.Basy;
    }


    #endregion

}
