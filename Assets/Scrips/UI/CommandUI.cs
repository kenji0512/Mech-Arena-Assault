using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーが操作するUI
public class CommandUI : MonoBehaviour
{
    public static CommandUI Instance;
    private System.Action<ICommand> _onCommandSelected;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _defendButton;
    [SerializeField] private Button _waitButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _panel.SetActive(false);
    }
    public void Open(PlayerUnit player, System.Action<ICommand> onSelected)
    {
        this._onCommandSelected = onSelected;
        _panel.SetActive(true);

        _attackButton.onClick.RemoveAllListeners();
        _attackButton.onClick.AddListener(() =>
        {
            _onCommandSelected?.Invoke(new AttackCommand());
            //Close();
        });

        _defendButton.onClick.RemoveAllListeners();
        _defendButton.onClick.AddListener(() =>
        {
            onSelected?.Invoke(new DefendCommand());
            Close();
        });
        _waitButton.onClick.RemoveAllListeners();
        _waitButton.onClick.AddListener(() =>
        {
            _onCommandSelected?.Invoke(new WaitCommand());
            Close();
        });
        // 他のコマンドも同様に…
    }
    public void Close()
    {
        _panel.SetActive(false);
    }
}
