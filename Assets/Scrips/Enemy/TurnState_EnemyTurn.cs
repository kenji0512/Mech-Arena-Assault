using UnityEngine;

public class TurnState_EnemyTurn : ITurnState
{
    private TurnManager _turnManager;

    public TurnState_EnemyTurn(TurnManager _turnManager)
    {
        this._turnManager = _turnManager;
    }

    public void Enter()
    {
        Debug.Log("敵のターン開始");

        foreach (var enemy in GameObject.FindObjectsOfType<EnemyUnit>())
        {
            enemy.ExecuteEnemyTurn();
        }

        _turnManager.ChangeState(BattleState.PlayerTurn);
    }

    public void Execute() { }

    public void Exit() { }
}