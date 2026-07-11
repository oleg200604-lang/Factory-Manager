using UnityEngine;

public class ZoneScr : MonoBehaviour
{
    public Vector2Int A; 
    public Vector2Int D; 

    public Vector2Int B => new Vector2Int(D.x, A.y);
    public Vector2Int C => new Vector2Int(A.x, D.y);

    public int Width => D.x - A.x + 1;
    public int Height => D.y - A.y + 1;

    public int Area => Width * Height;

    public bool Contains(Vector2Int point)
    {
        return point.x >= A.x &&
               point.x <= D.x &&
               point.y >= A.y &&
               point.y <= D.y;
    }
}