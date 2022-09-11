using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackBuildingData : BuildingData
{
    public List<SoldierUnit> Units;
}
public class BarrackUnit : BaseBuilding
{
    [SerializeField] private BaseTile SpawnPoint;
    [SerializeField] private float ProductionTimePerUnit = 1.0f;
    [SerializeField] private int MaxUnitProduced = 10;
    [SerializeField] private List<SoldierUnit> SoldiersToSpawn = new List<SoldierUnit>();

    public int _totalUnitsProduced = 0;
    private void Start()
    {
        StartCoroutine(ProduceUnits());
    }
    private IEnumerator ProduceUnits()
    {
        while (true)
        {

            for (int i = 0; i < SoldiersToSpawn.Count; i++)
            {
                if (_totalUnitsProduced >= MaxUnitProduced)
                {
                    yield break;
                }
                else
                {
                    _totalUnitsProduced++;
                    var soldierToSpawn = SoldiersToSpawn[i];
                    var soldier = ObjectPoolingManager.Instance.Spawn(soldierToSpawn.gameObject, SpawnPoint.GetPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(ProductionTimePerUnit);
                }
            }
        }
    }
    public override BuildingData GetBuildingData()
    {
        return new BarrackBuildingData
        {
            Name = this.BuildingName,
            Icon = this.GetIcon(),
            Units = this.SoldiersToSpawn
        };
    }

    public void SetSpawnPoint(BaseTile spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }

}
