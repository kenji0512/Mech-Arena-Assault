using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyUnit : UnitBase
{
    public int _ShootPower;
    public int _MeleePower;

    private IEnemyAI enemyAI;
    public AIBehaviorData _behaviorData;    // Inspector �Őݒ肷��
    public AIBehaviorData GetIBehaviorData() => _behaviorData;     // ����� DefaultEnemyAI �����痘�p����
    protected override void Awake()
    {
        base.Awake();
        enemyAI = new DefaultEnemyAI();
    }

    protected override void OnUnitDestroyed()
    {
        base.OnUnitDestroyed();
        if (GameObject.FindObjectsOfType<EnemyUnit>().Length <= 1)
        {
            FindAnyObjectByType<TurnManager>().SetBattleResult(true);
            FindAnyObjectByType<TurnManager>().ChangeState(BattleState.BattleEnd);
        }
        _IsDestroyed = true;
        gameObject.SetActive(false); // �܂��̓A�j���[�V�����Đ��Ȃ�
    }

    public async UniTask ExecuteEnemyTurn(PlayerUnit targetPlayer)
    {
        Vector2Int start = _GridPosition;
        Vector2Int goal = targetPlayer._GridPosition;

        var path = Pathfinding.Instance.FindPath(start, goal);

        if (path.Count == 0)
        {
            Debug.Log($"{UnitName} �͈ړ��ł��Ȃ��I");
            return;
        }

        int moveCount = Mathf.Min(_MoveRange, path.Count);
        Vector2Int moveTo = path[moveCount - 1];

        //Vector2Int �� Tile �ɕϊ�
        Tile targetTile = MapManager.Instance.GetTileAt(moveTo);

        if (targetTile == null || targetTile.IsBlocked)
        {
            Debug.Log($"{UnitName} �������ȃ}�X�Ɉړ����悤�Ƃ���: {moveTo}");
            return;
        }

        await MoveToTile(targetTile);
    }

    // �GAI�̐؂�ւ���̂��Ƃ̋����g�����\
    public void SetAI(IEnemyAI newAI)
    {
        enemyAI = newAI;
    }
    public UnitBase GetTargetRange()
    {        // �������F��ԋ߂��G���擾����Ȃ�
        var targets = FindObjectsOfType<PlayerUnit>();
        UnitBase nearst = null;
        float minDist = float.MaxValue;
        foreach (var item in targets)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                nearst = item;
            }
        }
        return nearst;
    }
}
