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
    }
    protected virtual void OnUnitDestroyed()
    {
        Debug.Log($"{UnitName} は撃破された！");

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
