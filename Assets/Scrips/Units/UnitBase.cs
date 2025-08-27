using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class UnitBase : MonoBehaviour
{
    //�}�b�v�ˑ��֌W
    public Tile CurrentTile { get; private set; }
    public Vector2Int _GridPosition => CurrentTile != null ? CurrentTile._GridPosition : _GridPositionBackup;
    private Vector2Int _GridPositionBackup; // Tile���ݒ莞�̈ꎞ�ۑ��p

    //�X�e�[�^�X�n
    public Dictionary<PartType, PartStatus> Parts = new();
    public bool _IsDestroyed { get; protected set; } = false;
    [SerializeField] protected MechData mechData;
    public System.Action<UnitBase> OnDestroyed;

    //�ȈՃv���p�e�B
    public bool IsDead => Parts.ContainsKey(PartType.Head) && Parts[PartType.Head].IsBroken;
    public string UnitName => mechData.unitName;

    [Header("�s���p�����[�^")]
    public int _MoveRange = 3;
    public int _AttackRange = 2;

    protected virtual void Awake()
    {
        InitParts();
    }
    //�^�C���Ɋ��蓖��
    public void SetTile(Tile tile)
    {
        if (CurrentTile != null)
            CurrentTile.RemoveUnit();

        CurrentTile = tile;
        _GridPositionBackup = tile._GridPosition;
        tile.PlaceUnit(this);

        transform.position = tile.transform.position;
    }
    //�^�C���ɃX�i�b�v���Ȃ���X���[�Y�ړ�
    public async UniTask MoveToTile(Tile tile)
    {
        if (tile == null || tile.IsBlocked)
        {
            Debug.LogWarning($"{UnitName}: �ړ��ł��Ȃ�Tile���w�肳�ꂽ");
            return;
        }

        // �Â��^�C������폜
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
        Debug.Log($"{UnitName} �� {part} �� {damage} �_���[�W�I�i�c��HP: {status.currentHP}�j");


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
        Debug.Log($"{UnitName} �� {part} ���j�󂳂ꂽ�I");
        // �v���C���[�Ȃ� UI�A�G�Ȃ� AI �ɒʒm�Ȃ�
        _IsDestroyed = true;
        OnDestroyed?.Invoke(this);  // �R�[���o�b�N�ʒm
    }
    protected virtual void OnUnitDestroyed()
    {
        Debug.Log($"{UnitName} �͌��j���ꂽ�I");
        _IsDestroyed = true;
        OnDestroyed?.Invoke(this);
        // �Q�[�����珜�O�A���o�Đ��Ȃ�
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
    // �s���\���̔���
    public bool CanMove => Parts.ContainsKey(PartType.Leg) && !Parts[PartType.Leg].IsBroken;
    public bool CanShoot => Parts.ContainsKey(PartType.RightArm) && !Parts[PartType.RightArm].IsBroken;
    public bool CanMelee => Parts.ContainsKey(PartType.LeftArm) && !Parts[PartType.LeftArm].IsBroken;

}
