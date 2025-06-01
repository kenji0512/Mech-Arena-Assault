using System.ComponentModel.Design;
using System.Windows.Input;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public bool HasSelectedCommand { get; private set; } = false;
    public ICommand SelectedCommand { get; private set; }


    protected override void Awake()
    {
        base.Awake();        // �v���C���[��p�������Ȃ�

    }
    protected override void OnUnitDestroyed()
    {
        base.OnUnitDestroyed();
        IsDestroyed = true;
        gameObject.SetActive(false); // �܂��̓A�j���[�V�����Đ��Ȃ�
    }

    public void SelectCommand()
    {
        HasSelectedCommand = false;
        // UI���J���ăv���C���[�ɓ��͂�����
        CommandUI.Instance.Open(this, command =>
        {
            SelectedCommand = command;
            HasSelectedCommand = true;
        });
        // �{���� UI ����I�΂��邪�A����͑�����
        Debug.Log($"{gameObject.name} ���U���R�}���h��I��");
        HasSelectedCommand = true;
    }

    public void ExecutePlayerAction()
    {
        Debug.Log($"{gameObject.name} ���U���I");
        // �G�Ƀ_���[�W��^���鏈���Ȃ�
        if (!HasSelectedCommand)
        {
            Debug.LogWarning("�R�}���h���I���ł�");
            return;
        }
        // ���s�����i�ړ��E�U���Ȃǁj

        // �ړ���U���̃R�}���h���s�����������ɒǉ��\��
        Debug.Log($"{UnitName} ���R�}���h�����s���܂���");
    }
    // UI�A�g����͏����p���\�b�h�ȂǕK�v�ɉ����Ēǉ�
    public void ResetCommand()
    {
        HasSelectedCommand = false;
    }
}
