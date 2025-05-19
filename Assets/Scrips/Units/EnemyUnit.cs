using UnityEngine;

public class EnemyUnit : UnitBase
{
    private IEnemyAI enemyAI;
    protected override void Awake()
    {
        base.Awake(); 
        enemyAI = new DefaultEnemyAI();
    }
    public void ExcutEnemyTurn()
    {
        enemyAI?.DecideAction(this);
    }
    // �GAI�̐؂�ւ���̂��Ƃ̋����g�����\
    public void SetAI(IEnemyAI newAI)
    {
        enemyAI = newAI;
    }
}
