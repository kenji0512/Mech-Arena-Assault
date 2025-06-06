using System;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    private EnemyUnit _targetEnemy;
    private Action<EnemyUnit> _onSelected;

    public void Setup(EnemyUnit enemy, Action<EnemyUnit> onSelected)
    {
        _targetEnemy = enemy;
        _onSelected = onSelected;
        GetComponent<Button>().onClick.RemoveAllListeners(); // �O�̂��ߏ�����
        // �����̃{�^���������ꂽ���ɌĂ΂�鏈��
        GetComponent<Button>().onClick.AddListener(() =>
        {
            _onSelected?.Invoke(_targetEnemy);
        });
    }
}
