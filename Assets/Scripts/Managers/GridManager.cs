
using System.Collections.Generic;
using System.Linq;
using Scripts.Bases;
using Scripts.Tiles;
using UnityEngine;
namespace Scripts.Managers
{

    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2 GridSize;
        [SerializeField] private GameObject Tile;

        public static GridManager Instance;
        private List<BaseTile> _tileList = new List<BaseTile>();
        private Bounds _bound;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            _bound = new Bounds();
        }

        public void GenerateGrid()
        {
            for (int i = 0; i < GridSize.x; i++)
            {
                for (int k = 0; k < GridSize.y; k++)
                {
                    var x = i == 0 ? 0 : i * .32f;
                    var y = k == 0 ? 0 : k * .32f;
                    Vector3 tilePosition = new Vector3(x, y, 1);

                    GameObject tile = ObjectPoolingManager.Instance.Spawn(Tile, tilePosition, Quaternion.identity);
                    tile.name = $"Tile {tilePosition.x}x{tilePosition.y}";
                    tile.transform.SetParent(transform);
                    GroundTile tileObj = tile.GetComponent<GroundTile>();
                    tileObj.SetPosition(tilePosition);
                    tileObj.SetColorByPosition(new Vector2(i, k));
                    _tileList.Add(tileObj);

                    tileObj.OnTileSelected += UIManager.Instance.SpawnBuilding;
                    tileObj.OnTileSelected += UnitManager.Instance.MoveUnit;
                    tileObj.OnPointerOver += UnitManager.Instance.ShowIndicator;
                }
            }
            CalculateBounds();
        }

        private void CalculateBounds()
        {
            foreach (var item in _tileList)
            {
                _bound.Encapsulate(item.GetComponent<BoxCollider2D>().bounds);
            }
        }

        public Bounds GetBounds()
        {
            return _bound;
        }

        public BaseTile GetTile(Vector2 position)
        {
            return _tileList.Where(t => Vector2.Distance(t.GetPosition(), position) <= .01f).First();
        }

        public List<BaseTile> GetTilesWithinBounds(Bounds bounds)
        {
            var list = new List<BaseTile>();
            Vector2 lowerLeft = new Vector2(bounds.center.x - bounds.extents.x + .16f, bounds.center.y - bounds.extents.y + .16f);

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    float x = lowerLeft.x + (i * .32f);
                    float y = lowerLeft.y + (k * .32f);
                    var pos = new Vector2(x, y);
                    var tile = GetTile(pos);
                    if (tile)
                    {
                        list.Add(tile);
                    }
                }
            }
            return list;
        }
        internal Vector2 GetGridSize()
        {
            return GridSize;
        }
    }
}