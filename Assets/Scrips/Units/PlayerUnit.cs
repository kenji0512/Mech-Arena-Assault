using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Mキーでテスト
        {
            var pos = await InputManager.Instance.WaitForTileClick(new List<Vector2Int> {
            new Vector2Int(2, 2),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3)
        });

            Debug.Log($"クリックしたのは {pos}");
        }
    }
    protected override void OnUnitDestroyed()
    {
        base.OnUnitDestroyed();
        _IsDestroyed = true;
        gameObject.SetActive(false); // またはアニメーション再生など
    }
    public async UniTask SelectMove()
    {
        // ① 既存ハイライトを全部消す
        MapManager.Instance.ClearHighlights();

        // ② 敵の攻撃範囲を赤で表示
        var enemies = FindObjectsOfType<EnemyUnit>();
        foreach (var enemy in enemies)
        {
            if (enemy._IsDestroyed) continue;

            var attackTiles = MapManager.Instance.GetAttackRangeTiles(enemy._GridPosition, enemy._AttackRange);
            MapManager.Instance.HighlightTiles(attackTiles, TileHighlightType.Attack);
        }

        // ③ プレイヤーの移動可能範囲を青で表示
        var movableTiles = MapManager.Instance.GetReachableTiles(_GridPosition, _MoveRange);
        MapManager.Instance.HighlightTiles(movableTiles, TileHighlightType.Move);

        // ④ プレイヤーがクリックしたタイルを待つ（自作 InputManager などを利用）
        Vector2Int targetGrid = await InputManager.Instance.WaitForTileClick(movableTiles);

        // ⑤ 実際に移動処理
        Tile targetTile = MapManager.Instance.GetTileAt(targetGrid);
        await MoveToTile(targetTile);

        // ⑥ ハイライトを消す（必要なら残してもOK）
        MapManager.Instance.ClearHighlights();
    }

    public void SelectCommand()
    {
        Debug.Log("SelectCommand() 呼び出された");

        HasSelectedCommand = false;
        // UIを開いてプレイヤーに入力させる
        CommandUI.Instance.Open(this, command =>
        {
            SelectedCommand = command;
            //HasSelectedCommand = true;
            Debug.Log($"{gameObject.name} が {command.GetType().Name} を選択");
            var uiManager = GameObject.FindObjectOfType<TargetSelectionUIManager>();

            if (command is AttackCommand)
            {
                // 攻撃範囲マスをハイライト
                var attackTiles = MapManager.Instance.GetAttackRangeTiles(_GridPosition, _AttackRange); // ←自ユニットの位置と射程
                MapManager.Instance.HighlightTiles(attackTiles);

                // 敵リストを取得
                var enemies = GameObject.Find("EnemyUnits").GetComponentsInChildren<EnemyUnit>(true);
                var enemyList = new List<EnemyUnit>(enemies);

                // UIを開いてターゲットを選ばせる
                uiManager.Show(enemyList, enemy =>
                {
                    _selectedTarget = enemy;
                    HasSelectedCommand = true;
                    Debug.Log($"{name} のターゲットが {enemy.name} に決定された！");

                    // UI 閉じる
                    uiManager.AttackCommand_Close();
                    CommandUI.Instance.Close();
                    MapManager.Instance.ClearHighlights(); // ハイライトもクリア
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
        SelectedCommand?.Execute(this);
        ResetCommand(); // 忘れずにリセット
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
