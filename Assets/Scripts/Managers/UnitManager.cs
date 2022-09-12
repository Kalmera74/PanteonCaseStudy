using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    [SerializeField] private SpriteRenderer BuildingIndicator;
    [SerializeField] private List<GameObject> BuildingsToSpawn = new List<GameObject>();
    private List<BaseBuilding> _buildings = new List<BaseBuilding>();

    private BaseUnit _selectedUnit;
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
        BaseBuilding building = buildingObj.GetComponent<BaseBuilding>();
        var spawnPosition = CalculateSpawnPositionBySize(spawnPoint.GetPosition(), building.GetSize());
        building = ObjectPoolingManager.Instance.Spawn(buildingObj, spawnPosition, Quaternion.identity).GetComponent<BaseBuilding>();
        building.SetPosition(spawnPosition);
        spawnPoint.SetOccupier(building);
        OccupiedTiles.Add(spawnPoint);
        building.SetSpawnPoint(spawnPoint);


        Vector2 buildingSize = building.GetSize();
        var occupiedTiles = CalculateOccupyingTiles(spawnPosition, building);
        building.SetOccupiedTiles(occupiedTiles);
        BuildingIndicator.gameObject.SetActive(false);
       
    }

    private List<BaseTile> CalculateOccupyingTiles(Vector2 spawnPoint, BaseBuilding building)
    {
        var bounds = building.GetBounds();
        return GridManager.Instance.GetTilesWithinBounds(bounds);
    }

    private Vector2 CalculateSpawnPositionBySize(Vector2 tilePosition, Vector2 buildingSize)
    {

        return tilePosition + new Vector2(.16f, .16f);

    }

    internal void ShowIndicator(BaseTile tile)
    {
        if (GameManager.Instance.GetState() == GameState.BuildingSelectedToSpawn)
        {
            var b = BuildingsToSpawn[UIManager.Instance.GetSelectedItemIndex()].GetComponent<BaseBuilding>();

            ShowIndicatorAtSpawnPoint(tile, b.GetSize());
        }
    }

    private void ShowIndicatorAtSpawnPoint(BaseTile spawnPoint, Vector2 size)
    {
        BuildingIndicator.gameObject.SetActive(true);
        BuildingIndicator.transform.localScale = size;
        BuildingIndicator.transform.position = CalculateSpawnPositionBySize(spawnPoint.GetPosition(), size);
    }

    internal void MoveUnit(BaseTile tile)
    {
        if (tile.GetOccupier() == null && _selectedUnit != null && _selectedUnit.GetType() == typeof(SoldierUnit))
        {
            SoldierUnit soldier = (SoldierUnit)_selectedUnit;
            soldier.MoveTo(tile);
            _selectedUnit = null;
        }
    }

    public List<BaseBuilding> GetBuildingsList()
    {
        return _buildings;
    }
    public void DeselectSelectedUnit()
    {
        if (_selectedUnit != null)
        {
            _selectedUnit.HideSelection();
            _selectedUnit = null;
        }
    }
    internal void SelectedUnit(BaseUnit unit)
    {
        if (_selectedUnit != null)
        {
            _selectedUnit.HideSelection();
        }
        _selectedUnit = unit;

        if (_selectedUnit.GetType() == typeof(BarrackUnit))
        {
            BarrackUnit barrack = (BarrackUnit)_selectedUnit;
            UIManager.Instance.ShowBuildingInfo(barrack.GetBuildingData());
        }
        else if (_selectedUnit.GetType() == typeof(PowerPlantUnit))
        {
            PowerPlantUnit barrack = (PowerPlantUnit)_selectedUnit;
            UIManager.Instance.ShowBuildingInfo(barrack.GetBuildingData());
        }
        else
        {
            UIManager.Instance.HideInfoPanel();
        }
    }
}
