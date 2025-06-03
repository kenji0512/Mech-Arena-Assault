using UnityEngine;

public enum AIActiopnPriority
{
    RangedFirst,
    MeleeFirst,
    Passive
}

[CreateAssetMenu(fileName = "AIBehaviorData", menuName = "AI/BehaviorData")]
public class AIBehaviorData : ScriptableObject
{
    public AIActiopnPriority _actionPriority;
    // 他に条件、行動パターンなどもここに追加
}
//ScriptableObject の利点
//プレハブやシーンを汚さず、複数の敵に共有可能な行動パターンを定義できる。

//データの変更が即座にすべての参照先に反映される。

//テストやデバッグがしやすい