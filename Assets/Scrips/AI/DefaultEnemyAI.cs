using UnityEngine;

public class DefaultEnemyAI : IEnemyAI
{
    public void DecideAction(EnemyUnit unit)
    {
        if (unit.CanShoot)
        {
            Debug.Log($"{unit.UnitName} は遠距離攻撃を選択！");
            var behavior = unit.GetIBehaviorData();
            if (behavior == null)
            {
                Debug.LogWarning($"{unit.UnitName} に AIBehaviorData がありません。デフォルト行動にします。");
                DefaultBehavior(unit);
                return;
            }
            switch (behavior._actionPriority)
            {
                case AIActiopnPriority.RangedFirst:
                    if (unit.CanShoot)
                    {
                        RangedAttack(unit);            // 遠距離攻撃処理
                    }
                    else if (unit.CanMelee)
                    {
                        MeleeAttack(unit);            // 近接攻撃処理
                    }
                    else
                    {
                        Wait(unit);                   // 待機処理
                    }
                    break;

                case AIActiopnPriority.MeleeFirst:
                    if (unit.CanMelee)
                    {
                        MeleeAttack(unit);
                    }
                    else if (unit.CanShoot)
                    {
                        RangedAttack(unit);
                    }
                    else
                    {
                        Wait(unit);
                    }
                    break;

                case AIActiopnPriority.Passive:
                    Wait(unit);
                    break;
            }
        }
    }
    private void RangedAttack(EnemyUnit unit)
    {
        var target = unit.GetTargetRange();
        if (target == null)
        {
            //Debug.LogWarning("ターゲットが見つかりません");


            Wait(unit);
            return;
        }
        int power = unit._ShootPower;

        Debug.Log($"{unit.UnitName} は遠距離攻撃を実行！");
        var action = new AttackAction(unit, target, unit._ShootPower);
        GameManager.Instance.actionExcuter.ExcuteAction(action);
    }

    private void MeleeAttack(EnemyUnit unit)
    {
        var target = unit.GetTargetRange();
        if (target == null)
        {
            Wait(unit);
            return;
        }

        Debug.Log($"{unit.UnitName} は近接攻撃を選択！");
        var action = new MeleeAction(unit, target, unit._MeleePower);
        GameManager.Instance.actionExcuter.ExcuteAction(action);
    }
    private void Wait(EnemyUnit unit)
    {
        if (unit.CanShoot)
            RangedAttack(unit);
        else if (unit.CanMelee)
            MeleeAttack(unit);
        else
            Wait(unit);
    }
    private void DefaultBehavior(EnemyUnit unit)
    {
        if (unit.CanShoot)
            RangedAttack(unit);
        else if (unit.CanMelee)
            MeleeAttack(unit);
        else
            Wait(unit);
    }
}
