using UnityEngine;

public class PlayerUnit : UnitBase
{
    public bool HasSelectedCommand {  get; private set; }

    protected override void Awake()
    {
        base.Awake();        // プレイヤー専用初期化など

    }
    public void SelectCommand()
    {
        HasSelectedCommand = true;
        Debug.Log($"{UnitName} がコマンドを選択しました");

    }
    public void ResetCommand()
    {
        HasSelectedCommand= false;
    }
    public void ExcutePlayerAction()
    {
        if(!HasSelectedCommand)
        {
            Debug.LogWarning("コマンド未選択です");
            return;
        }
        // 実行処理（移動・攻撃など）


    }
    // UI連携や入力処理用メソッドなど必要に応じて追加

}
