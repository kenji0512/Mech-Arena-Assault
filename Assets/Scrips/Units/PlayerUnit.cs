using UnityEngine;

public class PlayerUnit : UnitBase
{
    public ICommand SelectedCommand { get; private set; }
    public bool IsDefending { get; set; } = false;
    public bool HasSelectedCommand { get; private set; } = false;

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
            Debug.Log($"{gameObject.name} �� {command.GetType().Name} ��I��");
        });
        // �{���� UI ����I�΂��邪�A����͑�����
        //Debug.Log($"{gameObject.name} ���U���R�}���h��I��");
        //HasSelectedCommand = true;

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
        SelectedCommand = null;
        HasSelectedCommand = false;
        IsDefending = false;// �^�[���I�����Ƀ��Z�b�g
    }
    public override void TakeDamage(PartType part, int damage)
    {
        if (IsDefending)
        {
            damage /= 2; // �h�䒆�̓_���[�W����
            Debug.Log($"{name} �͖h�䂵�Ă������߁A�_���[�W�𔼌��I");
        }

        base.TakeDamage(part, damage);
    }

}
