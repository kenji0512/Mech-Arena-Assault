using UnityEngine;

public class DefaultEnemyAI : IEnemyAI
{
    public void DecideAction(EnemyUnit unit)
    {
        if (unit.CanShoot)
        {
            Debug.Log($"{unit.UnitName} �͉������U����I���I");
            // �������U������
        }
        else if (unit.CanMelee)
        {
            Debug.Log($"{unit.UnitName} �͋ߐڍU����I���I");
            // �ߐڍU������
        }
        else
        {
            Debug.Log($"{unit.UnitName} �͑ҋ@�I");
        }
    }
}
