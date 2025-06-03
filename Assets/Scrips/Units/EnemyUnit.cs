using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public int _ShootPower;
    public int _MeleePower;

    private IEnemyAI enemyAI;
    public AIBehaviorData _behaviorData;    // Inspector �Őݒ肷��
    public AIBehaviorData GetIBehaviorData() => _behaviorData;     // ����� DefaultEnemyAI �����痘�p����
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
    public UnitBase GetTargetRange()
    {        // �������F��ԋ߂��G���擾����Ȃ�
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
