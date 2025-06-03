using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public int _ShootPower;
    public int _MeleePower;

    private IEnemyAI enemyAI;
    public AIBehaviorData _behaviorData;    // Inspector で設定する
    public AIBehaviorData GetIBehaviorData() => _behaviorData;     // これを DefaultEnemyAI 側から利用する
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
        IsDestroyed = true;
        gameObject.SetActive(false); // またはアニメーション再生など
    }

    public void ExecuteEnemyTurn()
    {
        enemyAI?.DecideAction(this);
    }
    // 敵AIの切り替えや個体ごとの挙動拡張も可能
    public void SetAI(IEnemyAI newAI)
    {
        enemyAI = newAI;
    }
    public UnitBase GetTargetRange()
    {        // 仮実装：一番近い敵を取得するなど
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
