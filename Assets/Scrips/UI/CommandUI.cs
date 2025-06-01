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

        // ボタンにイベントを設定（例: 攻撃ボタン）
        _attackButton.onClick.RemoveAllListeners();
        _attackButton.onClick.AddListener(() =>
        {
            _onCommandSelected?.Invoke(new AttackCommand());
            Close();
        });

        // 他のコマンドも同様に…
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
