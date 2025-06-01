using UnityEngine;
using UnityEngine.UI;

public class CommandUI : MonoBehaviour
{
    public static CommandUI Instance;
    private System.Action<ICommand> _onCommandSelected;

    [SerializeField] private Button _attackButton;
    private void Awake()
    {
        Instance = this;
    }
    public void Open(PlayerUnit player, System.Action<ICommand> onSelected)
    {
        this._onCommandSelected = onSelected;
        gameObject.SetActive(true);

        // �{�^���ɃC�x���g��ݒ�i��: �U���{�^���j
        _attackButton.onClick.RemoveAllListeners();
        _attackButton.onClick.AddListener(() =>
        {
            _onCommandSelected?.Invoke(new AttackCommand());
            Close();
        });

        // ���̃R�}���h�����l�Ɂc
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
