using UnityEngine;
using UnityEngine.UI;
using System;

// プレイヤーが操作するUI
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

        // ボタンにイベントを設定（例: 攻撃ボタン）
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
        // 他のコマンドも同様に…
    }
    public void Close()
    {
        _root.SetActive(false);
    }
}
