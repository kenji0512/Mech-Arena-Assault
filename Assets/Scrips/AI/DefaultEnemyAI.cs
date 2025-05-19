using UnityEngine;

public class DefaultEnemyAI : IEnemyAI
{
    public void DecideAction(EnemyUnit unit)
    {
        if (unit.CanShoot)
        {
            Debug.Log($"{unit.UnitName} は遠距離攻撃を選択！");
            // 遠距離攻撃処理
        }
        else if (unit.CanMelee)
        {
            Debug.Log($"{unit.UnitName} は近接攻撃を選択！");
            // 近接攻撃処理
        }
        else
        {
            Debug.Log($"{unit.UnitName} は待機！");
        }
    }
}
