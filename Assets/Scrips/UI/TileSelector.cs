using UnityEngine;

public class TileSelector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.Highlight(TileHighlightType.Move);
                    Debug.Log($"Clicked Tile at {tile._GridPosition}");
                    // 移動範囲ハイライトとか呼び出す
                }
            }
        }
    }
}
