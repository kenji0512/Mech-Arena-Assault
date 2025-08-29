using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("�O���b�h�T�C�Y")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("�^�C���v���n�u")]
    [SerializeField] private GameObject _tilePrefab;

    // �O���b�h�̃^�C�����Ǘ��i���W �� Tile�j
    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();

    // �V���O���g��������
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
    // ���W����^�C�����擾
    public Tile GetTileAt(Vector2Int pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            return tile;
        return null;
    }
    // �w����W���^�C���͈͓����`�F�b�N
    public bool HasTile(Vector2Int pos)
    {
        return _tiles.ContainsKey(pos);
    }
    // �O���b�h���W�����[���h���W�ϊ�
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.y);
    }
    // ���[���h���W���O���b�h���W�ϊ�
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));
    }
    //ignoreBlock �� true/false �؂�ւ�������,���ۂɕ�����͈�,�˒������͈̔�
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
        // �ړ��̓u���b�N�𖳎����Ȃ��iignoreBlock = false�j
        return GetTilesInRange(startPos, moveRange, false);
    }
    public List<Vector2Int> GetMovableTiles(Vector2Int startPos, int moveRange)
    {
        return GetReachableTiles(startPos, moveRange);
    }
    public List<Vector2Int> GetAttackRangeTiles(Vector2Int startPos, int range)
    {
        // ignoreBlock = true �ŕǂ̌��������܂߂�
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
    // ���ׂẴ^�C���̃n�C���C�g������
    public void ClearHighlights()
    {
        foreach (var tile in _tiles.Values)
            tile.ResetColor();
    }
    // �O�����������W�̃^�C���̃u���b�N��Ԃ�ݒ�
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
            Debug.LogWarning($"�^�C�� {gridPos} �ɔz�u�ł��܂���");
            return null;
        }

        var unit = Instantiate(prefab, tile.transform.position, Quaternion.identity);
        tile.PlaceUnit(unit);
        return unit;
    }
}
