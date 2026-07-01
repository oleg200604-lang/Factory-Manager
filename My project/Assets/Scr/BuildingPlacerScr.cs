using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacerScr : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask groundLayer;
    public Material validMaterial;
    public Material invalidMaterial;

    private BuildingData selectedBuilding;
    private GameObject ghostInstance;
    private Vector2Int currentGridPos;
    private bool canPlace;
    private int rotationSteps = 0;

    public LayerMask buildingLayer; // шар, на якому стоять самі будівлі
    private bool demolishMode = false;
    Vector2Int GetRotatedSize()
    {
        bool swapped = rotationSteps % 2 == 1;
        return swapped
            ? new Vector2Int(selectedBuilding.gridSize.y, selectedBuilding.gridSize.x)
            : selectedBuilding.gridSize;
    }

    public void SelectBuilding(BuildingData building)
    {
        selectedBuilding = building;
        rotationSteps = 0;

        if (ghostInstance != null) Destroy(ghostInstance);
        ghostInstance = Instantiate(building.ghostPrefab);
        SetGhostMaterial(ghostInstance, validMaterial);
    }
    public void ToggleDemolishMode()
    {
        demolishMode = !demolishMode;

        // знесення і будівництво одночасно не мають сенсу
        if (demolishMode)
        {
            CancelPlacement();
        }
    }

    void HandleDemolish()
    {
        if (!demolishMode) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildingLayer))
        {
            PlacedBuildingInfoScr info = hit.collider.GetComponentInParent<PlacedBuildingInfoScr>();

            // підсвітку target-будівлі тут можна додати окремо (outline shader тощо)

            if (Input.GetMouseButtonDown(0) && info != null)
            {
                GridManagerScr.Instance.OccupyCells(info.gridOrigin, info.gridSize, false);
                Destroy(info.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            demolishMode = false;
        }
    }
    void Update()
    {
        if (selectedBuilding == null || ghostInstance == null)
        {
            HandleDemolish();
            return;
        }

        // Спочатку поворот — потім позиція з урахуванням нового розміру
        if (Input.GetKeyDown(KeyCode.R))
            rotationSteps = (rotationSteps + 1) % 4;

        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0))
            TryPlaceBuilding();

        if (Input.GetMouseButtonDown(1))
            CancelPlacement();
    }

    void UpdateGhostPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector2Int rotatedSize = GetRotatedSize();
            Vector2Int cursorCell = GridManagerScr.Instance.WorldToGrid(hit.point);

            // Зміщуємо origin назад на половину розміру — центр будівлі буде на cursorCell
            Vector2Int halfSize = new Vector2Int(rotatedSize.x / 2, rotatedSize.y / 2);
            currentGridPos = cursorCell - halfSize;

            // Позиція привида = центр footprint'у
            Vector3 originWorld = GridManagerScr.Instance.GridToWorld(currentGridPos);
            Vector3 center = originWorld + new Vector3(
                (rotatedSize.x - 1) * GridManagerScr.Instance.cellSize / 2f,
                0f,
                (rotatedSize.y - 1) * GridManagerScr.Instance.cellSize / 2f
            );

            ghostInstance.transform.position = center;
            ghostInstance.transform.rotation = Quaternion.Euler(0, rotationSteps * 90f, 0);

            canPlace = GridManagerScr.Instance.AreCellsFree(currentGridPos, rotatedSize)
                    && GridManagerScr.Instance.AreCellsOnPlatform(currentGridPos, rotatedSize);

            SetGhostMaterial(ghostInstance, canPlace ? validMaterial : invalidMaterial);
        }
    }


    void TryPlaceBuilding()
    {
        if (!canPlace) return;

        Vector3 finalPos = ghostInstance.transform.position;
        Quaternion finalRot = Quaternion.Euler(0, rotationSteps * 90f, 0);

        GameObject placed = Instantiate(selectedBuilding.prefab, finalPos, finalRot);

        // Зберігаємо дані для майбутнього знесення
        var info = placed.AddComponent<PlacedBuildingInfoScr>();
        info.gridOrigin = currentGridPos;
        info.gridSize = GetRotatedSize();
        info.buildingData = selectedBuilding;

        GridManagerScr.Instance.OccupyCells(currentGridPos, GetRotatedSize(), true);

        rotationSteps = 0; // скидаємо для наступної будівлі, якщо треба
    }

    void CancelPlacement()
    {
        if (ghostInstance != null) Destroy(ghostInstance);
        selectedBuilding = null;
        rotationSteps = 0; // ← додати
    }

    void SetGhostMaterial(GameObject obj, Material mat)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            renderer.material = mat;
        }
    }
}
