using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Mech/MechData")]
public class MechData : ScriptableObject
{
    public string unitName;
    public List<PartData> parts = new();
}
