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
    // “GAI‚ÌØ‚è‘Ö‚¦‚âŒÂ‘Ì‚²‚Æ‚Ì‹““®Šg’£‚à‰Â”\
    public void SetAI(IEnemyAI newAI)
    {
        enemyAI = newAI;
    }
}
