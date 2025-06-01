using UnityEngine;

public class AttackCommand : ICommand
{
    private int _DamageAmount = 10;
    public void Execute(UnitBase executor)
    {
        //仮実装　：　とりあえず敵を見つけて一体攻撃
        EnemyUnit enemy = GameObject.FindObjectOfType<EnemyUnit>();
        if (enemy != null)
        {
            Debug.Log($"{executor.name} が {enemy.name} に攻撃！");
            enemy.TakeDamage(PartType.Head , _DamageAmount); // 仮のダメージ処理
        }
    }
}