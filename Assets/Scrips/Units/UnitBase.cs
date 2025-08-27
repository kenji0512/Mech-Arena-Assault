using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class UnitBase : MonoBehaviour
{
    //マップ依存関係
    public Tile CurrentTile { get; private set; }
    public Vector2Int _GridPosition => CurrentTile != null ? CurrentTile._GridPosition : _GridPositionBackup;
    private Vector2Int _GridPositionBackup; // Tile未設定時の一時保存用

    //ステータス系
    public Dictionary<PartType, PartStatus> Parts = new();
    public bool _IsDestroyed { get; protected set; } = false;
    [SerializeField] protected MechData mechData;
    public System.Action<UnitBase> OnDestroyed;

    //簡易プロパティ
    public bool IsDead => Parts.ContainsKey(PartType.Head) && Parts[PartType.Head].IsBroken;
    public string UnitName => mechData.unitName;

    [Header("行動パラメータ")]
    public int _MoveRange = 3;
    public int _AttackRange = 2;

    protected virtual void Awake()
    {
        InitParts();
    }
    //タイルに割り当て
    public void SetTile(Tile tile)
    {
        if (CurrentTile != null)
            CurrentTile.RemoveUnit();

        CurrentTile = tile;
        _GridPositionBackup = tile._GridPosition;
        tile.PlaceUnit(this);

        transform.position = tile.transform.position;
    }
    //タイルにスナップしながらスムーズ移動
    public async UniTask MoveToTile(Tile tile)
    {
        if (tile == null || tile.IsBlocked)
        {
            Debug.LogWarning($"{UnitName}: 移動できないTileを指定された");
            return;
        }

        // 古いタイルから削除
        if (CurrentTile != null)
            CurrentTile.RemoveUnit();

        CurrentTile = tile;
        _GridPositionBackup = tile._GridPosition;
        tile.PlaceUnit(this);

        Vector3 worldPos = tile.transform.position;
        await transform.DOMove(worldPos, 0.5f).ToUniTask();
    }

    public virtual void TakeDamage(PartType part, int damage)
    {
        if (!Parts.ContainsKey(part)) return;

        PartStatus status = Parts[part];
        status.currentHP = Mathf.Max(0, status.currentHP - damage);
        Parts[part] = status;
        Debug.Log($"{UnitName} の {part} に {damage} ダメージ！（残りHP: {status.currentHP}）");


        if (status.IsBroken)
        {
            OnPartBroken(part);
        }

        if (part == PartType.Body && status.currentHP <= 0)
        {
            OnUnitDestroyed();
        }
    }
    protected virtual void OnPartBroken(PartType part)
    {
        Debug.Log($"{UnitName} の {part} が破壊された！");
        // プレイヤーなら UI、敵なら AI に通知など
        _IsDestroyed = true;
        OnDestroyed?.Invoke(this);  // コールバック通知
    }
    protected virtual void OnUnitDestroyed()
    {
        Debug.Log($"{UnitName} は撃破された！");
        _IsDestroyed = true;
        OnDestroyed?.Invoke(this);
        // ゲームから除外、演出再生など
    }

    protected virtual void InitParts()
    {
        Parts.Clear();
        foreach (var part in mechData.parts)
        {
            Parts[part.type] = new PartStatus
            {
                maxHP = part.maxHP,
                currentHP = part.maxHP
            };
        }
    }
    // 行動可能かの判定
    public bool CanMove => Parts.ContainsKey(PartType.Leg) && !Parts[PartType.Leg].IsBroken;
    public bool CanShoot => Parts.ContainsKey(PartType.RightArm) && !Parts[PartType.RightArm].IsBroken;
    public bool CanMelee => Parts.ContainsKey(PartType.LeftArm) && !Parts[PartType.LeftArm].IsBroken;

}
