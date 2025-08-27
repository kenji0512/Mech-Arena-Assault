using UnityEngine;
using System.Collections.Generic;

public class AttackCommand : ICommand
{
    private int _damageAmount = 10;

    public async void Execute(UnitBase executor)
    {
        if (executor is not PlayerUnit player) return;

        // 攻撃範囲内の敵ユニットを取得
        List<EnemyUnit> enemiesInRange = GetEnemiesInRange(player);

        if (enemiesInRange.Count == 0)
        {
            Debug.Log("攻撃範囲内に敵がいません！");
            return;
        }

        // UIを表示して選ばせる
        TargetSelectionUIManager ui = GameObject.FindObjectOfType<TargetSelectionUIManager>();
        ui.Show(enemiesInRange, (selectedEnemy) =>
        {
            Debug.Log($"{executor.name} が {selectedEnemy.name} に攻撃！");
            selectedEnemy.TakeDamage(PartType.Body, _damageAmount); // 部位は仮
        });
    }

    private List<EnemyUnit> GetEnemiesInRange(PlayerUnit player)
    {
        List<EnemyUnit> result = new();
        var allEnemies = GameObject.FindObjectsOfType<EnemyUnit>();
        List<Vector2Int> highlightTiles = new(); // ← これを追加！

        foreach (var enemy in allEnemies)
        {
            int dist = Mathf.Abs(player._GridPosition.x - enemy._GridPosition.x) +
                       Mathf.Abs(player._GridPosition.y - enemy._GridPosition.y);

            if (dist <= 3) // 例：射程3マス
            {
                result.Add(enemy);
                highlightTiles.Add(enemy._GridPosition);// ハイライト対象として
            }
        }
        MapManager.Instance.HighlightTiles(highlightTiles); // 攻撃範囲をハイライト

        return result;
    }
}
