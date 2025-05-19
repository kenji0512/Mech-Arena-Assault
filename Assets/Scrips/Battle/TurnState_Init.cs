using UnityEngine;

public class TurnState_Init : ITurnState
{
    private TurnManager _turnManager;
    public TurnState_Init(TurnManager turnManager)
    {
        this._turnManager = turnManager;
    }
    public void Enter()
    {
        Debug.Log("バトル初期化中...");
        // ユニット初期化など
        _turnManager.ChangeState(BattleState.PlayerTurn);
    }
    public void Execute() { }

    public void Exit() { }
}
