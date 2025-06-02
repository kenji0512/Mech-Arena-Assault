using UnityEngine;
using UnityEngine.UI;
using System;

// �v���C���[�����삷��UI
public class CommandUI : MonoBehaviour
{
    public static CommandUI Instance;
    private System.Action<ICommand> _onCommandSelected;

    [SerializeField] private GameObject _root;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button defendButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _root.SetActive(false);
    }
    public void Open(PlayerUnit player, System.Action<ICommand> onSelected)
    {
        this._onCommandSelected = onSelected;
        _root.SetActive(true);

        // �{�^���ɃC�x���g��ݒ�i��: �U���{�^���j
        _attackButton.onClick.RemoveAllListeners();
        _attackButton.onClick.AddListener(() =>
        {
            _onCommandSelected?.Invoke(new AttackCommand());
            Close();
        });

        defendButton.onClick.RemoveAllListeners();
        defendButton.onClick.AddListener(() =>
        {
            onSelected?.Invoke(new DefendCommand());
            Close();
        });
        // ���̃R�}���h�����l�Ɂc
    }
    public void Close()
    {
        _root.SetActive(false);
    }
}
