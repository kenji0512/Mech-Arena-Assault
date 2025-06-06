using System.Collections.Generic;
using UnityEditor.Splines;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public ICommand SelectedCommand { get; private set; }
    public bool IsDefending { get; set; } = false;

    private EnemyUnit _selectedTarget;

    public bool HasSelectedCommand { get; private set; } = false;
    [SerializeField] private TargetSelectionUIManager _targetUI;


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
            //HasSelectedCommand = true;
            Debug.Log($"{gameObject.name} が {command.GetType().Name} を選択");
            var uiManager = GameObject.FindObjectOfType<TargetSelectionUIManager>();

            if ( command is AttackCommand)
            {
                // 敵リストを取得
                var enemies = GameObject.Find("EnemyUnits").GetComponentsInChildren<EnemyUnit>(true);
                var enemyList = new List<EnemyUnit>(enemies);
                // UIを開いてターゲットを選ばせる
                uiManager.Show(enemyList, enemy =>
                {
                    _selectedTarget = enemy;
                    HasSelectedCommand = true;
                    Debug.Log($"{name} のターゲットが {enemy.name} に決定された！");
                    uiManager.AttackCommand_Close();
                    CommandUI.Instance.Close();
                });
            }
            else
            {            
                // 防御など他のコマンドならここで完了
                HasSelectedCommand = true;
            }
        });
        //SelectedCommand = new AttackCommand();
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
