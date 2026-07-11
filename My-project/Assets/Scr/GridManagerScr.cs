using UnityEngine;

public class GridManagerScr : MonoBehaviour
{
    public Vector2Int mapSize;
    public Build[,] builds;
    public Build building;
    public Camera cam;
    public void StartBuilding(Build selectBuild)
    {
        if (building != null)
        {
            Destroy(building.gameObject);
        }

        building = Instantiate(selectBuild);
    }
    private void Start()
    {
        builds = new Build[mapSize.x, mapSize.y];
    }
    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawCube(transform.position + new Vector3(x, 0.05f, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    private void Update()
    {
        if (building != null)
        {
            var groundPanel = new Plane(Vector3.up, Vector3.zero);
            Ray rey = cam.ScreenPointToRay(Input.mousePosition);

            if (groundPanel.Raycast(rey, out float pos))
            {
                Vector3 worldPos = rey.GetPoint(pos);

                int x = Mathf.RoundToInt(worldPos.x);
                int y = Mathf.RoundToInt(worldPos.z);

                building.transform.position = new Vector3(x, 0, y);
                if (y < 0 || y > mapSize.y - building.size.y || x < 0 || x > mapSize.x - building.size.x || isMapFree(x,y))
                {
                    building.SetTransform(false);
                }
                else
                {
                    building.SetTransform(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        for (int X = 0; X< building.size.x; X++)
                        {
                            for (int Y = 0; Y < building.size.x; Y++)
                            {
                                builds[x + X, y + Y] = building;
                            }

                        }

                        building.SetBuild();
                        building = null;
                    }
                }
            }
        }
    }
    private bool isMapFree(int x, int y)
    {
        for (int X = 0; X < building.size.x; X++)
        {
            for (int Y = 0; Y < building.size.y; Y++)
            {
                if (builds[x + X, y + Y] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
