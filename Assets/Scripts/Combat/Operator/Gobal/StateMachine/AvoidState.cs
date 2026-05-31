using UnityEngine;

public class AvoidState : ICombatState
{
    private PlayerMovementControl _playerMovementControl;
    private CombatStateMachineBase _combatStateMachine;
    private Animator _animator;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    // Unity对接参数：完美闪避慢动作倍率和残影节奏，后续如果需要调参可以改为从SO或控制器传入。
    private const float TimeScale = 0.4f;
    private const float MeshRefreshRate = 0.2f;
    private const float MeshDestroyDelay = 1f;
    private const float AvoidSlowEndNormalizedTime = 0.5f;
    private static Material _afterimageMaterial;

    private float _meshRefreshTimer;
    private float _originalTimeScale = 1f;
    private float _originalFixedDeltaTime = 0.02f;
    private bool _isRunning;
    private bool _isTimeSlowActive;
    private bool _hasEnteredAvoidAnimation;

    public AvoidState(PlayerMovementControl playerMovementControl, CombatStateMachineBase combatStateMachine)
    {
        _playerMovementControl = playerMovementControl;
        _combatStateMachine = combatStateMachine;
        _animator = playerMovementControl.animator;
        _skinnedMeshRenderers = playerMovementControl.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void OnEnter()
    {
        if (_playerMovementControl == null || _animator == null)
            return;

        _isRunning = true;
        _hasEnteredAvoidAnimation = false;

        // 一开始不要立刻生成残影，避免刚切入闪避时残影贴在角色身上。
        _meshRefreshTimer = MeshRefreshRate;

        if (_skinnedMeshRenderers == null || _skinnedMeshRenderers.Length == 0)
            _skinnedMeshRenderers = _playerMovementControl.GetComponentsInChildren<SkinnedMeshRenderer>();

        _animator.CrossFadeInFixedTime("Avoid", 0f, 0, 0);
        TeamManager.Instance.teamCurrentEnergy += 25f;
        StartTimeSlow();
    }

    public void OnEnterAgain()
    {
        OnEnter();
    }

    public void OnExit()
    {
        StopTimeSlow();
        _isRunning = false;
        _hasEnteredAvoidAnimation = false;
    }

    public void OnUpdate()
    {
        if (!_isRunning || _animator == null)
            return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Avoid"))
            _hasEnteredAvoidAnimation = true;

        // 只在完美闪避前半段做全局时间减缓和残影，复刻旧版 normalizedTime < 0.5 的窗口。
        if (stateInfo.IsTag("Avoid") && stateInfo.normalizedTime < AvoidSlowEndNormalizedTime)
        {
            _meshRefreshTimer -= Time.unscaledDeltaTime;
            if (_meshRefreshTimer <= 0f)
            {
                CreateAfterimage();
                _meshRefreshTimer = MeshRefreshRate;
            }

            return;
        }

        StopTimeSlow();

        // 等Avoid动画真正离开后再回默认状态，避免提前打断后半段闪避动画。
        if (_hasEnteredAvoidAnimation && !stateInfo.IsTag("Avoid"))
        {
            _combatStateMachine?.ReturnToDefaultState();
        }
    }

    private void StartTimeSlow()
    {
        if (_isTimeSlowActive)
            return;

        _originalTimeScale = Time.timeScale;
        _originalFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = _originalFixedDeltaTime * TimeScale;
        _isTimeSlowActive = true;
    }

    private void StopTimeSlow()
    {
        if (!_isTimeSlowActive)
            return;

        Time.timeScale = _originalTimeScale;
        Time.fixedDeltaTime = _originalFixedDeltaTime;
        _isTimeSlowActive = false;
    }

    private void CreateAfterimage()
    {
        if (_playerMovementControl == null || _skinnedMeshRenderers == null)
            return;

        for (int i = 0; i < _skinnedMeshRenderers.Length; i++)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderers[i];
            if (skinnedMeshRenderer == null)
                continue;

            GameObject afterimage = new GameObject($"{_playerMovementControl.name}_AvoidAfterimage");
            // BakeMesh生成的是该SkinnedMeshRenderer当前姿势的网格快照，用Renderer自身Transform还原人物部件位置。
            afterimage.transform.SetPositionAndRotation(skinnedMeshRenderer.transform.position, skinnedMeshRenderer.transform.rotation);
            afterimage.transform.localScale = Vector3.one;

            MeshRenderer meshRenderer = afterimage.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = afterimage.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(mesh, true);
            meshFilter.mesh = mesh;

            // 每个submesh都使用同一个透明发光材质，避免多材质模型只显示头发、衣服等局部。
            Material[] materials = new Material[Mathf.Max(1, mesh.subMeshCount)];
            for (int j = 0; j < materials.Length; j++)
                materials[j] = GetAfterimageMaterial();
            meshRenderer.sharedMaterials = materials;

            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;

            Object.Destroy(mesh, MeshDestroyDelay);
            Object.Destroy(afterimage, MeshDestroyDelay);
        }
    }

    private static Material GetAfterimageMaterial()
    {
        if (_afterimageMaterial != null)
            return _afterimageMaterial;

        Shader shader = Resources.Load<Shader>("Shaders/AvoidAfterimageGlow");
        if (shader == null)
            shader = Shader.Find("Kingdom/Avoid Afterimage Glow");
        if (shader == null)
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
            shader = Shader.Find("Unlit/Color");

        _afterimageMaterial = new Material(shader);
        _afterimageMaterial.name = "Avoid Afterimage Glow Blue";

        Color color = new Color(0.05f, 0.55f, 1f, 0.28f);
        if (_afterimageMaterial.HasProperty("_BaseColor"))
            _afterimageMaterial.SetColor("_BaseColor", color);
        if (_afterimageMaterial.HasProperty("_Color"))
            _afterimageMaterial.SetColor("_Color", color);
        if (_afterimageMaterial.HasProperty("_RimColor"))
            _afterimageMaterial.SetColor("_RimColor", new Color(0.15f, 0.85f, 1f, 1f));
        if (_afterimageMaterial.HasProperty("_Alpha"))
            _afterimageMaterial.SetFloat("_Alpha", 0.32f);
        if (_afterimageMaterial.HasProperty("_FresnelPower"))
            _afterimageMaterial.SetFloat("_FresnelPower", 2.2f);
        if (_afterimageMaterial.HasProperty("_EmissionIntensity"))
            _afterimageMaterial.SetFloat("_EmissionIntensity", 2.6f);

        _afterimageMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        return _afterimageMaterial;
    }
}
