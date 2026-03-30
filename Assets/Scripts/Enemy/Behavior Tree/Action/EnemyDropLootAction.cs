using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyDropLootAction : Action
{
    private LootSpawner _lootSpawner;
    private bool _hasDroppedLoot;

    public override void OnAwake()
    {
        base.OnAwake();
        _lootSpawner = GetComponent<LootSpawner>();
    }

    public override void OnStart()
    {
        _hasDroppedLoot = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (_hasDroppedLoot)
            return TaskStatus.Success;

        if (_lootSpawner != null)
        {
            _lootSpawner.CreatLootItem();
        }

        _hasDroppedLoot = true;
        return TaskStatus.Success;
    }
}
