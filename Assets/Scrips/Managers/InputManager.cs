using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private bool _isWaitingForClick = false;
    private List<Vector2Int> _validTiles;
    private Vector2Int _clickedTile;
    private Camera _cam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _cam = Camera.main;
        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager.Instance が null です！");
            return;
        }
    }

    /// <summary>
    /// 指定されたタイルの中からプレイヤーがクリックしたものを待つ
    /// </summary>
    public async UniTask<Vector2Int> WaitForTileClick(List<Vector2Int> validTiles)
    {
        _validTiles = validTiles;
        _isWaitingForClick = true;

        while (_isWaitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPos = MapManager.Instance.WorldToGrid(worldPos);

                if (_validTiles.Contains(gridPos))
                {
                    _clickedTile = gridPos;
                    _isWaitingForClick = false;
                }
            }
            await UniTask.Yield(); // 毎フレーム待機
        }

        return _clickedTile;
    }
}
