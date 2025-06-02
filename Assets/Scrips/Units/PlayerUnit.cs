using UnityEngine;

public class PlayerUnit : UnitBase
{
    public ICommand SelectedCommand { get; private set; }
    public bool IsDefending { get; set; } = false;
    public bool HasSelectedCommand { get; private set; } = false;

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
        HasSelectedCommand = false;
        // UIを開いてプレイヤーに入力させる
        CommandUI.Instance.Open(this, command =>
        {
            SelectedCommand = command;
            HasSelectedCommand = true;
            Debug.Log($"{gameObject.name} が {command.GetType().Name} を選択");
        });
        // 本来は UI から選ばせるが、今回は即決定
        //Debug.Log($"{gameObject.name} が攻撃コマンドを選択");
        //HasSelectedCommand = true;

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
        SelectedCommand = null;
        HasSelectedCommand = false;
        IsDefending = false;// ターン終了時にリセット
    }
    public override void TakeDamage(PartType part, int damage)
    {
        if (IsDefending)
        {
            damage /= 2; // 防御中はダメージ半減
            Debug.Log($"{name} は防御していたため、ダメージを半減！");
        }

        base.TakeDamage(part, damage);
    }

}
