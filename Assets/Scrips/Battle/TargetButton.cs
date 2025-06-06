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
        GetComponent<Button>().onClick.RemoveAllListeners(); // 念のため初期化
        // 自分のボタンが押された時に呼ばれる処理
        GetComponent<Button>().onClick.AddListener(() =>
        {
            _onSelected?.Invoke(_targetEnemy);
        });
    }
}
