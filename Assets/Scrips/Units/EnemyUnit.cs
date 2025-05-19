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
        gameObject.SetActive(false); // �܂��̓A�j���[�V�����Đ��Ȃ�
    }

    public void ExecuteEnemyTurn()
    {
        enemyAI?.DecideAction(this);
    }
    // �GAI�̐؂�ւ���̂��Ƃ̋����g�����\
    public void SetAI(IEnemyAI newAI)
    {
        enemyAI = newAI;
    }
}
