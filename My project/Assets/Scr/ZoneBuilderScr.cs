using UnityEngine;

public class ZoneBuilderScr : MonoBehaviour
{
    private Vector2Int? firstPoint;


    public void SelectCell(GridCellScr cell)
    {
        if (firstPoint == null)
        {
            firstPoint = cell.Position;

            Debug.Log("Перша точка: " + firstPoint);
        }
        else
        {
            Vector2Int secondPoint = cell.Position;

            CreateZone(firstPoint.Value, secondPoint);

            firstPoint = null;
        }
    }


    private void CreateZone(Vector2Int A, Vector2Int D)
    {
        ZoneScr zone = new ZoneScr();

        zone.A = new Vector2Int(
            Mathf.Min(A.x, D.x),
            Mathf.Min(A.y, D.y)
        );

        zone.D = new Vector2Int(
            Mathf.Max(A.x, D.x),
            Mathf.Max(A.y, D.y)
        );


        Debug.Log(
            $"Створена зона {zone.A} - {zone.D}"
        );
    }
}