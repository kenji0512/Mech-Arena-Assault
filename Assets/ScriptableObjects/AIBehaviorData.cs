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
    // ���ɏ����A�s���p�^�[���Ȃǂ������ɒǉ�
}
//ScriptableObject �̗��_
//�v���n�u��V�[�����������A�����̓G�ɋ��L�\�ȍs���p�^�[�����`�ł���B

//�f�[�^�̕ύX�������ɂ��ׂĂ̎Q�Ɛ�ɔ��f�����B

//�e�X�g��f�o�b�O�����₷��