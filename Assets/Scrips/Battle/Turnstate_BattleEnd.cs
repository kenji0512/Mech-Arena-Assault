using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnState_BattleEnd : ITurnState
{
    private TurnManager _turnManager;
    private float _elapsedTime = 0f;
    private float _waitDuration = 3f;

    public TurnState_BattleEnd(TurnManager turnManager)
    {
        this._turnManager = turnManager;
    }
    public void Enter()
    {
        Debug.Log("バトル終了");

        // 勝敗判定やリザルト画面への遷移など
        // ここでは仮にログ出力のみ

        if (_turnManager.IsPlayerWin)
        {
            Debug.Log("プレイヤーの勝利！");
        }
        else
        {
            Debug.Log("敵の勝利！");
        }

        // 必要に応じて数秒待ってからタイトル画面に戻すなど
        // SceneManager.LoadScene("ResultScene"); などもOK
    }

    public void Exit()
    {
        Debug.Log("バトルエンドステート終了");
    }

    void ITurnState.Execute()
    {
        // 例：一定時間後にリザルトシーンへ遷移する処理など
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _waitDuration)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
