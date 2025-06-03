using UnityEngine;

public class DefaultEnemyAI : IEnemyAI
{
    public void DecideAction(EnemyUnit unit)
    {
        if (unit.CanShoot)
        {
            Debug.Log($"{unit.UnitName} �͉������U����I���I");
            var behavior = unit.GetIBehaviorData();
            if (behavior == null)
            {
                Debug.LogWarning($"{unit.UnitName} �� AIBehaviorData ������܂���B�f�t�H���g�s���ɂ��܂��B");
                DefaultBehavior(unit);
                return;
            }
            switch (behavior._actionPriority)
            {
                case AIActiopnPriority.RangedFirst:
                    if (unit.CanShoot)
                    {
                        RangedAttack(unit);            // �������U������
                    }
                    else if (unit.CanMelee)
                    {
                        MeleeAttack(unit);            // �ߐڍU������
                    }
                    else
                    {
                        Wait(unit);                   // �ҋ@����
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
            //Debug.LogWarning("�^�[�Q�b�g��������܂���");


            Wait(unit);
            return;
        }
        int power = unit._ShootPower;

        Debug.Log($"{unit.UnitName} �͉������U�������s�I");
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

        Debug.Log($"{unit.UnitName} �͋ߐڍU����I���I");
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
