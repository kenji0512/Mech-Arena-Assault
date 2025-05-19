using UnityEngine;

public class TurnState_PlayerAction : ITurnState
{
    private TurnManager _turnManager;
    public TurnState_PlayerAction(TurnManager turnManager)
    {
        this._turnManager = turnManager;
    }
    public void Enter()
    {
        Debug.Log("プレイヤーの行動フェーズ開始");
        // 各プレイヤーに ExecutePlayerAction を呼ぶなど
        foreach (var player in GameObject.FindObjectsOfType<PlayerUnit>())
        {
            player.ExecutePlayerAction();
            player.ResetCommand();
        }

        // 少し待ってから敵ターンへ（今は即遷移）
        _turnManager.ChangeState(BattleState.EnemyTurn);
    }

    public void Execute() { }

    public void Exit() { }
}
