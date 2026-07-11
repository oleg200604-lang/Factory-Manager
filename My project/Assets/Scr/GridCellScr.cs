using UnityEngine;

public class GridCellScr : MonoBehaviour
{
    public Vector2Int Position;

    public void Initialize(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }
    private void OnMouseDown()
    {
        FindObjectOfType<ZoneBuilderScr>().SelectCell(this);
    }
}