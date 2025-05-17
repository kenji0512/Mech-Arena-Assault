using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public Dictionary<PartType, PartStatus> Parts = new();

    public void TakeDamage(PartType part, int damage)
    {
        if (Parts.TryGetValue(part, out PartStatus status))

        {
            status.currentHP = Mathf.Max(0, status.currentHP - damage);
            if (status.IsBroken)
            {
                OnPartBroken(part);
            }
        }
    }
    protected virtual void OnPartBroken(PartType part)
    {

    }
}
