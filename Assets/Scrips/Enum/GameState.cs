using UnityEngine;

public class GameState : MonoBehaviour
{

}
public enum PartType
{
    Head,
    Body,
    LeftArm,
    RightArm,
    Leg
}
[System.Serializable]
public class PartStatus
{
    public int maxHP;
    public int currentHP;
    public bool IsBroken => currentHP <= 0;
}
[System.Serializable]
public class PartData
{
    public PartType type;
    public int maxHP;
}

public enum BattleState
{
    Init,           // 初期化
    PlayerTurn,     // プレイヤー入力待ち
    PlayerAction,   // プレイヤー行動中
    EnemyTurn,      // 敵行動中
    BattleEnd       // 勝利 or 敗北
}