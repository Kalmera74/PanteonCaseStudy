using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GridManager GridManager;
    [SerializeField] private List<GameObject> BuildingsToSpawn = new List<GameObject>();
    private List<BaseBuilding> _buildings = new List<BaseBuilding>();

    public static UnitManager Instance;

    private void Awake()
    {
        for (int i = 0; i < BuildingsToSpawn.Count; i++)
        {
            var building = BuildingsToSpawn[i].GetComponent<BaseBuilding>();
            _buildings.Add(building);
        }
        Instance = this;
    }
    public void SpawnBuilding(BaseTile spawnPoint, int buildingIndex)
    {
        List<BaseTile> OccupiedTiles = new List<BaseTile>();
        GameObject buildingObj = BuildingsToSpawn[buildingIndex];
        BarrackUnit building = ObjectPoolingManager.Instance.Spawn(buildingObj, spawnPoint.GetPosition() + new Vector2(0.16f, 0.16f), Quaternion.identity).GetComponent<BarrackUnit>();

        Vector2 buildingSize = building.GetSize();
        spawnPoint.SetOccupier(building);
        OccupiedTiles.Add(spawnPoint);

        building.SetSpawnPoint(spawnPoint);
        // for (int i = 1; i < buildingSize.x - 1; i++)
        // {
        //     for (int k = 1; k < buildingSize.y - 1; k++)
        //     {
        //         var x = i * .32f;
        //         var y = k * .32f;
        //         var neighborTilePosition = spawnPoint.GetPosition() + (new Vector2(x, y));
        //         var neighborTile = GridManager.GetTile(neighborTilePosition);

        //         if (neighborTile != null)
        //         {
        //             neighborTile.SetOccupier(building);
        //             OccupiedTiles.Add(neighborTile);
        //         }
        //     }
        // }

        // building.SetOccupiedTiles(OccupiedTiles);
    }

    public List<BaseBuilding> GetBuildingsList()
    {
        return _buildings;
    }
}
