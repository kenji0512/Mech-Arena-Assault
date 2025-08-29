using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    private List<Vector2Int> attackRangeTiles;
    private PlayerUnit selectedUnit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 左クリック
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var unit = hit.collider.GetComponent<PlayerUnit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
                else
                {
                    DeselectUnit();
                }
            }
        }
        if (selectedUnit != null && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    //var movableTiles = MapManager.Instance.GetMovableTiles(selectedUnit._GridPosition, selectedUnit._MoveRange);
                    //if (movableTiles.Contains(tile.GridPosition))
                    //{
                    //    // 移動開始
                    //    selectedUnit.MoveToSmooth(tile.GridPosition).Forget();
                    //    MapManager.Instance.ClearHighlights();
                    //    selectedUnit = null;  // 移動したら選択解除
                    //}
                }
            }
        }
        // 攻撃モードなら
        if (attackRangeTiles != null && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var tile = hit.collider.GetComponent<Tile>();
                if (tile != null && attackRangeTiles.Contains(tile._GridPosition))
                {
                    // タイルに敵がいるかチェック
                    var enemy = FindEnemyAt(tile._GridPosition);
                    if (enemy != null)
                    {
                        // 攻撃処理
                        enemy.TakeDamage(PartType.Body, selectedUnit.CanMelee ? selectedUnit._MoveRange : selectedUnit._MoveRange);
                        // 攻撃後の処理（ターン終了やUI戻すなど）
                        attackRangeTiles = null;
                        MapManager.Instance.ClearHighlights();
                    }
                }
            }
        }
    }
    // 敵を位置から探す仮実装
    EnemyUnit FindEnemyAt(Vector2Int pos)
    {
        foreach (var enemy in GameObject.FindObjectsOfType<EnemyUnit>())
        {
            if (enemy._GridPosition == pos)
                return enemy;
        }
        return null;
    }
    void SelectUnit(PlayerUnit unit)
    {
        selectedUnit = unit;
        // 移動可能範囲のハイライト表示などを呼ぶ
        //MapManager.Instance.HighlightTiles(MapManager.Instance.GetMovableTiles(unit._GridPosition, unit._MoveRange));
    }

    void DeselectUnit()
    {
        // 何もない場所をクリックしたときに、選択解除＆ハイライト消去
        selectedUnit = null;
        MapManager.Instance.ClearHighlights();
    }
    void ShowAttackRange(UnitBase unit)
    {
        // 攻撃範囲をハイライトして attackRangeTiles に保存
        // 実際の攻撃クリック処理で使うための「攻撃モード」突入処理
        attackRangeTiles = MapManager.Instance.GetAttackRangeTiles(unit._GridPosition, unit._AttackRange);
        MapManager.Instance.HighlightTiles(attackRangeTiles);
    }
}
