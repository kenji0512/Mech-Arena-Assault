using UnityEngine;

public class TurnState_PlayerTurn : ITurnState
{
    private TurnManager _turnManager;
    private PlayerUnit[] _playerUnit;
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
        PlayerUnit[] _playerUnits = playerParent.GetComponentsInChildren<PlayerUnit>(true); // 非アクティブな子も含めるなら true

        // とりあえず全員に自動でコマンド選ばせる（後でUI連携）

        if (_playerUnit == null || _playerUnit.Length == 0)
        {
            Debug.LogWarning("プレイヤーユニットが存在しません！");
            return;
        }
        foreach (var player in _playerUnit)
        {
            player.SelectCommand(); // 仮の自動選択
        }
    }
    public void Execute()
    {        // 全ユニットがコマンド選択完了したら
        if (AllPlayerUnitsSelected())
        {
            _turnManager.ChangeState(BattleState.PlayerAction);
        }
    }
    public void Exit()
    {
        Debug.Log("プレイヤーターン終了");
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
