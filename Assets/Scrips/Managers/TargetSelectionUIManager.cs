using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _enemyButtonPrefab; // ボタンのプレハブ
    [SerializeField] private Button _confirmButton;         // 確認ボタン
    [SerializeField] private Transform _buttonParent;       // ボタン配置先

    private EnemyUnit _selectedTarget;
    private Action<EnemyUnit> _onTargetConfirmed;

    private List<GameObject> _instantiatedButtons = new(); // 生成したボタンのリスト

    public void Show(List<EnemyUnit> enemies, Action<EnemyUnit> onConfirmed)
    {
        //選択されるエネミーの情報入れ
        _panel.SetActive(true);
        _onTargetConfirmed = onConfirmed;
        _selectedTarget = null;
        _confirmButton.interactable = false;


        // 既存ボタン削除
        foreach (var child in _instantiatedButtons)
        {
            if (child.gameObject != _confirmButton.gameObject)
                Destroy(child.gameObject);
        }
        _instantiatedButtons.Clear();

        // 敵ごとにボタン生成
        foreach (var enemy in enemies)
        {
            GameObject newButtonObj = Instantiate(_enemyButtonPrefab, _buttonParent);
            var button = newButtonObj.GetComponent<TargetButton>();
            if (button != null)
            {
                EnemyUnit captured = enemy; // クロージャ対策
                button.Setup(captured, (selected) =>
                {
                    _selectedTarget = selected;
                    _confirmButton.interactable = true;
                    Debug.Log($"選択されたターゲット: {selected.name}");
                });
            }
            _instantiatedButtons.Add(newButtonObj);
        }

        // 確定ボタンがある場合
        if (_confirmButton != null)
        {
            _confirmButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.AddListener(() =>
            {
                if (_selectedTarget != null)
                {
                    _onTargetConfirmed?.Invoke(_selectedTarget);
                    _panel.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("ターゲットが選ばれていません");
                }
            });
        }
        _confirmButton.onClick.AddListener(() =>
        {
            if (_selectedTarget != null)
            {
                _onTargetConfirmed?.Invoke(_selectedTarget);
                _panel.SetActive(false);
                MapManager.Instance.ClearHighlights(); // ここで解除！
            }
        });
    }
    public void AttackCommand_Close()
    {
        _selectedTarget = null;
        _panel.SetActive(false);
        //gameObject.SetActive(false);
    }
}
