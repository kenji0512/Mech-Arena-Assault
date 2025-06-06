using System.Collections.Generic;
using UnityEditor.Splines;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public ICommand SelectedCommand { get; private set; }
    public bool IsDefending { get; set; } = false;

    private EnemyUnit _selectedTarget;

    public bool HasSelectedCommand { get; private set; } = false;
    [SerializeField] private TargetSelectionUIManager _targetUI;


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
            //HasSelectedCommand = true;
            Debug.Log($"{gameObject.name} �� {command.GetType().Name} ��I��");
            var uiManager = GameObject.FindObjectOfType<TargetSelectionUIManager>();

            if ( command is AttackCommand)
            {
                // �G���X�g���擾
                var enemies = GameObject.Find("EnemyUnits").GetComponentsInChildren<EnemyUnit>(true);
                var enemyList = new List<EnemyUnit>(enemies);
                // UI���J���ă^�[�Q�b�g��I�΂���
                uiManager.Show(enemyList, enemy =>
                {
                    _selectedTarget = enemy;
                    HasSelectedCommand = true;
                    Debug.Log($"{name} �̃^�[�Q�b�g�� {enemy.name} �Ɍ��肳�ꂽ�I");
                    uiManager.AttackCommand_Close();
                    CommandUI.Instance.Close();
                });
            }
            else
            {            
                // �h��ȂǑ��̃R�}���h�Ȃ炱���Ŋ���
                HasSelectedCommand = true;
            }
        });
        //SelectedCommand = new AttackCommand();
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
