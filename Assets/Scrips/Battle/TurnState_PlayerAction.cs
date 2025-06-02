using Cysharp.Threading.Tasks;
using UnityEngine;
//選ばれたコマンドを実行するフェーズ
public class TurnState_PlayerAction : ITurnState
{
    private TurnManager _turnManager;
    private PlayerUnit[] _playerUnits;
    private int _currentIndex = 0;
    public TurnState_PlayerAction(TurnManager turnManager)
    {
        this._turnManager = turnManager;
    }
    public void Enter()
    {
        Debug.Log("プレイヤーの行動フェーズ開始");
        // 各プレイヤーに ExecutePlayerAction を呼ぶなど
        //foreach (var player in GameObject.FindObjectsOfType<PlayerUnit>())
        //{
        //    player.ExecutePlayerAction();
        //    player.ResetCommand();
        //}

        //// 少し待ってから敵ターンへ（今は即遷移）
        //_turnManager.ChangeState(BattleState.EnemyTurn);
        _playerUnits = GameObject.Find("PlayerUnits").GetComponentsInChildren<PlayerUnit>();
        _currentIndex = 0;

        ExecuteNextActionsAsync();
    }

    private async UniTaskVoid ExecuteNextActionsAsync()
    {
        while (_currentIndex < _playerUnits.Length)
        {
            var _player = _playerUnits[_currentIndex];
            _player.SelectedCommand?.Execute(_player);
            _currentIndex++;
            // 少し待ってから敵ターンへ（今は即遷移）
            await UniTask.Delay(500);
        }

        Debug.Log("全プレイヤーの行動終了");
        _turnManager.ChangeState(BattleState.EnemyTurn); // 次のステートへ
    }

    public void Execute() { }

    public void Exit()
    {
        Debug.Log("プレイヤー行動フェーズ終了");
    }
}
