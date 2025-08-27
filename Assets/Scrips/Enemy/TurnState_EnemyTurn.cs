using Cysharp.Threading.Tasks;
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

        var players = GameObject.FindObjectsOfType<PlayerUnit>();

        foreach (var enemy in GameObject.FindObjectsOfType<EnemyUnit>())
        {
            // 最も近いプレイヤーを探す（単純な距離計算）
            PlayerUnit nearest = null;
            int minDist = int.MaxValue;

            foreach (var player in players)
            {
                int dist = Mathf.Abs(enemy._GridPosition.x - player._GridPosition.x) +
                           Mathf.Abs(enemy._GridPosition.y - player._GridPosition.y);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = player;
                }
            }

            if (nearest != null)
            {
                // ここで引数を渡す
                enemy.ExecuteEnemyTurn(nearest).Forget(); // async/await なしバージョン
            }
        }

        _turnManager.ChangeState(BattleState.PlayerTurn);
    }


    public void Execute() { }

    public void Exit() { }
}