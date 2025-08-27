using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Dictionary<BattleState, ITurnState> _states = new();
    private ITurnState _currentState;
    private readonly ReactiveProperty<BattleState> _state = new();
    private BattleManager _battleManager;

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
        _battleManager = FindObjectOfType<BattleManager>(); 
        ChangeState(BattleState.Init);
    }
    public bool IsBattleOver()
    {
        var playerUnits = _battleManager.GetPlayerUnits();
        var enemyUnits = _battleManager.GetEnemyUnits();

        bool allPlayersDead = playerUnits.TrueForAll(p => p.IsDead);
        bool allEnemiesDead = enemyUnits.TrueForAll(e => e.IsDead);

        if (allPlayersDead) { GameOver(false); return true; }
        if (allEnemiesDead) { GameOver(true); return true; }

        return false;
    }

    void GameOver(bool playerWon)
    {
        SetBattleResult(playerWon);
        ChangeState(BattleState.BattleEnd);
    }

    public void ChangeState(BattleState newState)
    {
        Debug.Log($"èÛë‘ëJà⁄: {_state.Value} Å® {newState}");

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
