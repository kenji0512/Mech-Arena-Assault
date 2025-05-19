using UnityEngine;

public class EnemyUnit : UnitBase
{
    private IEnemyAI enemyAI;
    protected override void Awake()
    {
        base.Awake(); 
        enemyAI = new DefaultEnemyAI();
    }
    protected override void OnUnitDestroyed()
    {
        base.OnUnitDestroyed();
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
}
