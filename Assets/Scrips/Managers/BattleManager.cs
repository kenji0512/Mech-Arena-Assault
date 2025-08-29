using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [Header("ユニットプレハブ")]
    [SerializeField] private List<GameObject> playerPrefabs = new();
    [SerializeField] private List<GameObject> enemyPrefabs = new();

    [Header("初期配置位置")]
    [SerializeField] private List<Vector2Int> playerSpawnPositions = new();
    [SerializeField] private List<Vector2Int> enemySpawnPositions = new();

    [Header("親オブジェクト")]
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform enemyParent;

    private List<PlayerUnit> _playerUnits = new();
    private List<EnemyUnit> _enemyUnits = new();

    private void Start()
    {
        InitUnits();
    }

    private void InitUnits()
    {
        // プレイヤー生成
        for (int i = 0; i < playerSpawnPositions.Count; i++)
        {
            GameObject prefab = playerPrefabs[Mathf.Min(i, playerPrefabs.Count - 1)];
            Vector2Int spawnPos = playerSpawnPositions[i];

            Tile spawnTile = MapManager.Instance.GetTileAt(spawnPos);
            if (spawnTile == null)
            {
                Debug.LogError($"Tile が存在しません: {spawnPos}");
                continue;
            }

            Vector3 spawnWorldPos = spawnTile.transform.position;
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
                spawnWorldPos.y += renderer.bounds.size.y / 2f;

            GameObject unitObj = Instantiate(prefab, spawnWorldPos, Quaternion.identity, playerParent);
            PlayerUnit unit = unitObj.GetComponent<PlayerUnit>();
            if (unit == null)
            {
                Debug.LogError($"PlayerPrefab {prefab.name} に PlayerUnit が付いてない！");
                Destroy(unitObj);
                continue;
            }

            unit.SetTile(spawnTile);
            _playerUnits.Add(unit);
        }

        // 敵生成
        for (int i = 0; i < enemySpawnPositions.Count; i++)
        {
            GameObject prefab = enemyPrefabs[Mathf.Min(i, enemyPrefabs.Count - 1)];
            Vector2Int spawnPos = enemySpawnPositions[i];

            Tile spawnTile = MapManager.Instance.GetTileAt(spawnPos);
            if (spawnTile == null)
            {
                Debug.LogError($"Tile が存在しません: {spawnPos}");
                continue;
            }

            Vector3 spawnWorldPos = spawnTile.transform.position;
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
                spawnWorldPos.y += renderer.bounds.size.y / 2f;
            Vector3 targetPos = _playerUnits.Count > 0
                ? _playerUnits[0].transform.position
                : Vector3.zero;

            GameObject unitObj = Instantiate(prefab, spawnWorldPos, Quaternion.identity, enemyParent);
            unitObj.transform.rotation = Quaternion.Euler(0, 180f, 0);
            EnemyUnit unit = unitObj.GetComponent<EnemyUnit>();
            if (unit == null)
            {
                Debug.LogError($"EnemyPrefab {prefab.name} に EnemyUnit が付いてない！");
                Destroy(unitObj);
                continue;
            }
            unit.SetTile(spawnTile);
            _enemyUnits.Add(unit);
        }
    }

    public List<PlayerUnit> GetPlayerUnits() => _playerUnits;
    public List<EnemyUnit> GetEnemyUnits() => _enemyUnits;
}
