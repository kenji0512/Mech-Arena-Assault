using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public Dictionary<PartType, PartStatus> Parts = new();
    public bool IsDestroyed { get; protected set; } = false;
    [SerializeField] protected MechData mechData;
    public string UnitName => mechData.unitName;

    protected virtual void Awake()
    {
        InitParts();
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
    }
    protected virtual void OnUnitDestroyed()
    {
        Debug.Log($"{UnitName} �͌��j���ꂽ�I");

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
