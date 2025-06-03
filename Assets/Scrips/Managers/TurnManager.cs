using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Dictionary<BattleState, ITurnState> _states = new();
    private ITurnState _currentState;
    private readonly ReactiveProperty<BattleState> _state = new();
    public bool IsPlayerWin { get; private set; }

    public IReadOnlyReactiveProperty<BattleState> State => _state;

    private void Awake()
    {
        _states[BattleState.Init] = new TurnState_Init(this);
        _states[BattleState.PlayerTurn] = new TurnState_PlayerTurn(this);
        _states[BattleState.PlayerAction] = new TurnState_PlayerAction(this);
        _states[BattleState.EnemyTurn] = new TurnState_EnemyTurn(this);
        _states[BattleState.BattleEnd] = new TurnState_BattleEnd(this);
    }
    private void Start()
    {
        ChangeState(BattleState.Init);
        _state.Subscribe(OnStateChanged).AddTo(this);
    }
    private void OnStateChanged(BattleState newState)
    {
        Debug.Log($"ó‘Ô‘JˆÚ: {newState}");
    }
    public void ChangeState(BattleState newState)
    {
        Debug.Log($"ó‘Ô‘JˆÚ: {_state.Value} ¨ {newState}");

        _currentState?.Exit();
        _currentState = _states[newState];
        _state.Value = newState;
        _currentState.Enter();
    }
    public void Update()
    {
        _currentState?.Execute();
    }
    public void SetBattleResult(bool playerWon)
    {
        IsPlayerWin = playerWon;
    }
}
