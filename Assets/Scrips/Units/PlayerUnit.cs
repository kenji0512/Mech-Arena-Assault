using UnityEngine;

public class PlayerUnit : UnitBase
{
    public bool HasSelectedCommand {  get; private set; }

    protected override void Awake()
    {
        base.Awake();        // �v���C���[��p�������Ȃ�

    }
    public void SelectCommand()
    {
        HasSelectedCommand = true;
        Debug.Log($"{UnitName} ���R�}���h��I�����܂���");

    }
    public void ResetCommand()
    {
        HasSelectedCommand= false;
    }
    public void ExcutePlayerAction()
    {
        if(!HasSelectedCommand)
        {
            Debug.LogWarning("�R�}���h���I���ł�");
            return;
        }
        // ���s�����i�ړ��E�U���Ȃǁj


    }
    // UI�A�g����͏����p���\�b�h�ȂǕK�v�ɉ����Ēǉ�

}
