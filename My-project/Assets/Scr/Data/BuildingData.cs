using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Buildings/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    public GameObject ghostPrefab; // напівпрозора версія
    public Vector2Int gridSize = Vector2Int.one; // 1x1, 2x2 тощо
}