using System.Collections;
using System.Collections.Generic;
using Scripts.Bases;
using Scripts.Managers;
using System.Linq;
using UnityEngine;

namespace Scripts.Units
{

    public class BarrackBuildingData : BuildingData
    {
        public List<SoldierUnit> Units;
    }
    public class BarrackUnit : BaseBuilding
    {
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
                        var spawnPoint = new Vector3(SpawnPoint.GetPosition().x, SpawnPoint.GetPosition().y, -1);
                        var soldier = ObjectPoolingManager.Instance.Spawn(soldierToSpawn.gameObject, spawnPoint, Quaternion.identity).GetComponent<SoldierUnit>();
                        soldier.SetPosition(GetPatrolTile());
                        yield return new WaitForSeconds(ProductionTimePerUnit);
                    }
                }
            }
        }

        private BaseTile GetPatrolTile()
        {
            var random = new System.Random();

            var tile = GetOccupiedTiles().OrderBy(e => random.Next()).Take(1).First();

            var offsetX = UnityEngine.Random.Range(0, 2);
            var offsetY = UnityEngine.Random.Range(0, 2);

            var pos = new Vector2(offsetX * .32f, offsetY * .32f) + tile.GetPosition();


            var newTile = GridManager.Instance.GetTile(pos);
            if (newTile)
            {
                return newTile;
            }
            return tile;
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

    }

}