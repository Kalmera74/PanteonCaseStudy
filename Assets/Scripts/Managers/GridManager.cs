using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int GridWidth;
    [SerializeField] private int GridHeight;
    [SerializeField] private GameObject Tile;

    private List<BaseTile> _tileList = new List<BaseTile>();
    private Bounds _bound;
    private void Start()
    {
        _bound = new Bounds();
    }

    public void GenerateGrid()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int k = 0; k < GridHeight; k++)
            {
                var x = i == 0 ? 0 : i * .32f;
                var y = k == 0 ? 0 : k * .32f;
                Vector2 tilePosition = new Vector2(x, y);
             
                GameObject tile = ObjectPoolingManager.Instance.Spawn(Tile, tilePosition, Quaternion.identity);
                tile.name = $"Tile {tilePosition.x}x{tilePosition.y}";
                tile.transform.SetParent(transform);
                GroundTile tileObj = tile.GetComponent<GroundTile>();
                tileObj.SetPosition(tilePosition);
                tileObj.SetColor(new Vector2(i, k));
                _tileList.Add(tileObj);
                tileObj.OnTileSelected += UIManager.Instance.SpawnBuilding;
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
        return _tileList.Where(t => t.GetPosition().Equals(position)).First();
    }
}
