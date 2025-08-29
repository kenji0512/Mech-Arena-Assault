using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("グリッドサイズ")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("タイルプレハブ")]
    [SerializeField] private GameObject _tilePrefab;

    // グリッドのタイルを管理（座標 → Tile）
    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();

    // シングルトン初期化
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tileObj = Instantiate(_tilePrefab, new Vector3(x, 0, y), Quaternion.Euler(90,0,0),transform);
                tileObj.name = $"Tile {x},{y}";

                var tile = tileObj.GetComponent<Tile>();
                tile.Initialize(new Vector2Int(x, y), false);

                _tiles[new Vector2Int(x, y)] = tile;
            }
        }
    }
    // 座標からタイルを取得
    public Tile GetTileAt(Vector2Int pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            return tile;
        return null;
    }
    // 指定座標がタイル範囲内かチェック
    public bool HasTile(Vector2Int pos)
    {
        return _tiles.ContainsKey(pos);
    }
    // グリッド座標→ワールド座標変換
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.y);
    }
    // ワールド座標→グリッド座標変換
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));
    }
    //ignoreBlock の true/false 切り替えだけで,実際に歩ける範囲,射程だけの範囲
    public List<Vector2Int> GetTilesInRange(Vector2Int startPos, int range, bool ignoreBlock)
    {
        var result = new List<Vector2Int>();
        var visited = new HashSet<Vector2Int>();
        var queue = new Queue<(Vector2Int pos, int cost)>();

        queue.Enqueue((startPos, 0));
        visited.Add(startPos);

        Vector2Int[] directions = {
        Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down
    };

        while (queue.Count > 0)
        {
            var (pos, cost) = queue.Dequeue();
            if (cost > range) continue;

            result.Add(pos);

            foreach (var dir in directions)
            {
                var next = pos + dir;
                if (visited.Contains(next)) continue;
                if (!HasTile(next)) continue;
                if (!ignoreBlock && _tiles[next].IsBlocked) continue;

                visited.Add(next);
                queue.Enqueue((next, cost + 1));
            }
        }
        return result;
    }
    public List<Vector2Int> GetReachableTiles(Vector2Int startPos, int moveRange)
    {
        // 移動はブロックを無視しない（ignoreBlock = false）
        return GetTilesInRange(startPos, moveRange, false);
    }
    public List<Vector2Int> GetMovableTiles(Vector2Int startPos, int moveRange)
    {
        return GetReachableTiles(startPos, moveRange);
    }
    public List<Vector2Int> GetAttackRangeTiles(Vector2Int startPos, int range)
    {
        // ignoreBlock = true で壁の向こうも含める
        return GetTilesInRange(startPos, range, true);
    }
    public void HighlightTiles(List<Vector2Int> tiles, TileHighlightType type = TileHighlightType.Move)
    {
        foreach (var pos in tiles)
        {
            var tile = GetTileAt(pos);
            if (tile != null)
                tile.Highlight(type);
        }
    }
    // すべてのタイルのハイライトを解除
    public void ClearHighlights()
    {
        foreach (var tile in _tiles.Values)
            tile.ResetColor();
    }
    // 外部から特定座標のタイルのブロック状態を設定
    public void SetTileBlocked(Vector2Int pos, bool blocked)
    {
        var tile = GetTileAt(pos);
        if (tile != null)
            tile.SetBlocked(blocked);
    }
    public UnitBase SpawnUnit(UnitBase prefab, Vector2Int gridPos)
    {
        var tile = GetTileAt(gridPos);
        if (tile == null || tile.IsBlocked)
        {
            Debug.LogWarning($"タイル {gridPos} に配置できません");
            return null;
        }

        var unit = Instantiate(prefab, tile.transform.position, Quaternion.identity);
        tile.PlaceUnit(unit);
        return unit;
    }
}
