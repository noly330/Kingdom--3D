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
    private MoveController moveController;
    private PlayerInput playerInput;

    [Header("残影特效相关")]
    private float meshDestoryDelay = 1f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Material trailMaterial;


    private float hitStunDuration = 0.2f;
    private bool canControl = true;
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
        //TODO：后续无敌要和闪避分开
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
        if (comboList == null)
        {
            normalComboList = boxingComboList;
            currentComboList = boxingComboList;
            return;
        }
        normalComboList = comboList;
        currentComboList = comboList;
    }

    private void CheckState()
    {
        if (currentCharacter.currentHealth <= 0.01f && !currentCharacter.isDead)
        {
            animator.SetBool("IsDead", true);
            currentCharacter.isDead = true;
            GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        }

        //TODO:用协程
        if (hitStunDuration > 0.02f)
        {
            if (canControl)
            {
                playerInput.actions.FindActionMap("Player").Disable();
                canControl = false;
            }
            hitStunDuration -= Time.deltaTime;
        }
        else
        {
            if (!canControl)
            {
                playerInput.actions.FindActionMap("Player").Enable();
                canControl = true;
            }
        }
    }

    private Coroutine perfectAvoidCoroutine;
    protected override void CharacterCombatBeHit(ComboInteractionConfig interactionConfig, CharacterBase attacker)
    {
        if (currentCharacter.isInvulnerable)
        {
            Debug.Log("闪避成功");
            currentCharacter.invulnerableTime += 0.15f;

            if (perfectAvoidCoroutine == null)
                perfectAvoidCoroutine = StartCoroutine(IE_PerfectAvoid());

            return;
        }
        hitStunDuration = 0.3f;
        base.CharacterCombatBeHit(interactionConfig, attacker);
        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        animator.Play(interactionConfig.hitName, 0, 0);
        //生成受击特效
        var fxObj = hitFXList[(int)interactionConfig.attackForce].TryGetOneFXObj();
        if (fxObj != null)
            ToolManager.instance.PlayOneFX(fxObj, hitPoints[0].position, Vector3.zero, new Vector3(1, 1, 1));
        //生成音效
    }

    public float perfectAvoidDuration = 1f;  // 完美闪避总时间
    public float meshRefreshRate = 0.3f;  // 残影生成间隔
    public float tScale = 0.3f;

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

}
