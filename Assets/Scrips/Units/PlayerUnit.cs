using UnityEngine;

public class PlayerUnit : UnitBase
{
    public bool HasSelectedCommand { get; private set; }

    protected override void Awake()
    {
        base.Awake();        // プレイヤー専用初期化など

    }
    protected override void OnUnitDestroyed()
    {
        base.OnUnitDestroyed();
        IsDestroyed = true;
        gameObject.SetActive(false); // またはアニメーション再生など
    }

    public void SelectCommand()
    {
        // 本来は UI から選ばせるが、今回は即決定
        Debug.Log($"{gameObject.name} が攻撃コマンドを選択");
        HasSelectedCommand = true;
    }

    public void ExecutePlayerAction()
    {
        Debug.Log($"{gameObject.name} が攻撃！");
        // 敵にダメージを与える処理など
        if (!HasSelectedCommand)
        {
            Debug.LogWarning("コマンド未選択です");
            return;
        }
        // 実行処理（移動・攻撃など）

        // 移動や攻撃のコマンド実行処理をここに追加予定
        Debug.Log($"{UnitName} がコマンドを実行しました");
    }
    // UI連携や入力処理用メソッドなど必要に応じて追加
    public void ResetCommand()
    {
        HasSelectedCommand = false;
    }
}
