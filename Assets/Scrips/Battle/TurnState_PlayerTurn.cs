using UniRx;
using UnityEngine;
//UIでコマンド選ばせるフェーズ
public class TurnState_PlayerTurn : ITurnState
{
    private TurnManager _turnManager;
    private PlayerUnit[] _playerUnit;
    private int _currentIndex = 0;
    public TurnState_PlayerTurn(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }
    public void Enter()
    {
        Debug.Log("プレイヤーのターン開始");
        // 入力を受け付け、コマンド選択開始
        // ここでPlayerUnitに選択させる
        Transform playerParent = GameObject.Find("PlayerUnits").transform;
        _playerUnit = playerParent.GetComponentsInChildren<PlayerUnit>(true);
        _currentIndex = 0;

        // とりあえず全員に自動でコマンド選ばせる（後でUI連携）

        if (_playerUnit == null || _playerUnit.Length == 0)
        {
            Debug.LogWarning("プレイヤーユニットが存在しません！");
            return;
        }
        StartSelectingNextUnit();
    }
    private void StartSelectingNextUnit()
    {
        if(_currentIndex >= _playerUnit.Length)
        {
            _turnManager.ChangeState(BattleState.PlayerAction);
            return;
        }

        PlayerUnit player = _playerUnit[_currentIndex];
        player.SelectCommand();

        //コマンド選択完了を監視(UniRx)
        Observable.EveryUpdate()
            .Where(_ => player.HasSelectedCommand)
            .Take(1)
            .Subscribe(_ =>
            {
                _currentIndex++;
                StartSelectingNextUnit();
            });
    }
    public void Execute()
    {        // 全ユニットがコマンド選択完了したら
        //if (AllPlayerUnitsSelected())
        //{
        //    _turnManager.ChangeState(BattleState.PlayerAction);
        //}
    }
    public void Exit()
    {
        Debug.Log("プレイヤー選択終了");
    }
    private bool AllPlayerUnitsSelected()
    {
        PlayerUnit[] players = GameObject.FindObjectsOfType<PlayerUnit>();
        foreach (var player in players)
        {
            if (!player.HasSelectedCommand) return false;
        }
        return true;
    }
}
