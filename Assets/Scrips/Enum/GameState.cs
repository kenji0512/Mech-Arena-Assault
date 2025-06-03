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
    Init,           // ������
    PlayerTurn,     // �v���C���[���͑҂�
    PlayerAction,   // �v���C���[�s����
    EnemyTurn,      // �G�s����
    BattleEnd       // ���� or �s�k
}