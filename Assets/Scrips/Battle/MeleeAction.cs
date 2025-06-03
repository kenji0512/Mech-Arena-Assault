using UnityEngine;

public class MeleeAction : IAction
{
    private EnemyUnit _attacker;
    private UnitBase _target;
    private int _power;

    public MeleeAction(EnemyUnit attacker, UnitBase target, int power)
    {
        this._attacker = attacker;
        this._target = target;
        this._power = power;
    }

    public void Execute()
    {
        Debug.Log($"{_attacker.UnitName} が {_target.UnitName} に近接攻撃！（威力：{_power}）");
        _target.TakeDamage(PartType.Body, _power);//引数は仮
    }
}
