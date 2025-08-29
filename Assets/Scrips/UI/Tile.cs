using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public Vector2Int _GridPosition { get; set; }
    //邪魔な元とかで通れない場所
    [SerializeField] private bool _isTerrainBlocked = false;
    // 現在ユニットが乗っているか
    public UnitBase OccupiedUnit { get; private set; }
    // 外部から参照する用：最終的にブロックされているかどうか
    public bool IsBlocked => _isTerrainBlocked || OccupiedUnit != null;

    private MeshRenderer _renderer;
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _highlightColor = Color.cyan;
    [SerializeField] private Color _attackRangeColor = Color.red;


    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<MeshRenderer>();
            if (_renderer == null)
                Debug.LogError($"{name} に MeshRendererが見つかりません！");
        }
    }
    // ユニットを設定
    public void SetUnit(UnitBase unit)
    {
        OccupiedUnit = unit;
        if (unit != null)
            unit.SetTile(this); // UnitBase 側の位置情報も更新
    }
    public void PlaceUnit(UnitBase unit)
    {
        if (OccupiedUnit != null)
        {
            Debug.LogWarning($"タイル {_GridPosition} はすでに {OccupiedUnit.name} がいます！");
            return;
        }

        OccupiedUnit = unit;
        unit.transform.position = transform.position; // タイルの位置に配置
        _isTerrainBlocked = true;
    }
    public void RemoveUnit()
    {
        OccupiedUnit = null;
        _isTerrainBlocked = false;
    }
    public void SetBlocked(bool blocked)
    {
        _isTerrainBlocked = blocked;

        // ブロックされている場合は暗く表示
        if (_renderer != null)
            _renderer.material.color = blocked ? Color.gray : _defaultColor;
    }
    // 移動範囲や攻撃範囲のハイライト
    public void Highlight(TileHighlightType type = TileHighlightType.Move)
    {
        if (_renderer == null) return;

        switch (type)
        {
            case TileHighlightType.Move:
                _renderer.material.color = _highlightColor;
                break;
            case TileHighlightType.Attack:
                _renderer.material.color = _attackRangeColor;
                break;
        }
    }
    //生成時に一括設定
    public void Initialize(Vector2Int gridPosition, bool isBlocked = false)
    {
        _GridPosition = gridPosition;
        _isTerrainBlocked = isBlocked;
        ResetColor();
    }
    public void ResetColor() => _renderer.material.color = _defaultColor;
}
