using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScr : MonoBehaviour
{
    public static GridManagerScr Instance { get; private set; }

    public float cellSize = 2f;
    public Vector3 originPosition = Vector3.zero;

    private Dictionary<Vector2Int, bool> occupiedCells = new Dictionary<Vector2Int, bool>();

    void Awake()
    {
        Instance = this;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - originPosition;
        int x = Mathf.FloorToInt(local.x / cellSize);
        int z = Mathf.FloorToInt(local.z / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return originPosition + new Vector3(
            gridPos.x * cellSize + cellSize / 2f,
            0f,
            gridPos.y * cellSize + cellSize / 2f
        );
    }
    // GridManager.cs — додаємо поле і методи

    private HashSet<Vector2Int> platformCells = new HashSet<Vector2Int>();

    // Викликати коли платформа з'являється в сцені (або з Awake, якщо статична)
    public void RegisterPlatformCells(Vector2Int origin, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
            for (int z = 0; z < size.y; z++)
                platformCells.Add(origin + new Vector2Int(x, z));
    }

    // Якщо платформа може зникати — зняти реєстрацію
    public void UnregisterPlatformCells(Vector2Int origin, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
            for (int z = 0; z < size.y; z++)
                platformCells.Remove(origin + new Vector2Int(x, z));
    }

    public bool AreCellsOnPlatform(Vector2Int origin, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
            for (int z = 0; z < size.y; z++)
                if (!platformCells.Contains(origin + new Vector2Int(x, z)))
                    return false;
        return true;
    }
    public bool IsCellFree(Vector2Int gridPos)
    {
        return !occupiedCells.ContainsKey(gridPos) || !occupiedCells[gridPos];
    }

    // Перевірка для будівель розміром більше 1x1
    public bool AreCellsFree(Vector2Int origin, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int cell = origin + new Vector2Int(x, z);
                if (!IsCellFree(cell)) return false;
            }
        }
        return true;
    }

    public void OccupyCells(Vector2Int origin, Vector2Int size, bool occupied)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int cell = origin + new Vector2Int(x, z);
                occupiedCells[cell] = occupied;
            }
        }
    }
}
