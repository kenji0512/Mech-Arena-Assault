using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _enemyButtonPrefab; // �{�^���̃v���n�u
    [SerializeField] private Button _confirmButton;         // �m�F�{�^��
    [SerializeField] private Transform _buttonParent;       // �{�^���z�u��

    private EnemyUnit _selectedTarget;
    private Action<EnemyUnit> _onTargetConfirmed;

    private List<GameObject> _instantiatedButtons = new(); // ���������{�^���̃��X�g

    public void Show(List<EnemyUnit> enemies, Action<EnemyUnit> onConfirmed)
    {
        //�I�������G�l�~�[�̏�����
        _panel.SetActive(true);
        _onTargetConfirmed = onConfirmed;
        _selectedTarget = null;
        _confirmButton.interactable = false;


        // �����{�^���폜
        foreach (var child in _instantiatedButtons)
        {
            if (child.gameObject != _confirmButton.gameObject)
                Destroy(child.gameObject);
        }
        _instantiatedButtons.Clear();

        // �G���ƂɃ{�^������
        foreach (var enemy in enemies)
        {
            GameObject newButtonObj = Instantiate(_enemyButtonPrefab, _buttonParent);
            var button = newButtonObj.GetComponent<TargetButton>();
            if (button != null)
            {
                EnemyUnit captured = enemy; // �N���[�W���΍�
                button.Setup(captured, (selected) =>
                {
                    _selectedTarget = selected;
                    _confirmButton.interactable = true;
                    Debug.Log($"�I�����ꂽ�^�[�Q�b�g: {selected.name}");
                });
            }
            _instantiatedButtons.Add(newButtonObj);
        }

        // �m��{�^��������ꍇ
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
                    Debug.LogWarning("�^�[�Q�b�g���I�΂�Ă��܂���");
                }
            });
        }
        _confirmButton.onClick.AddListener(() =>
        {
            if (_selectedTarget != null)
            {
                _onTargetConfirmed?.Invoke(_selectedTarget);
                _panel.SetActive(false);
                MapManager.Instance.ClearHighlights(); // �����ŉ����I
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
