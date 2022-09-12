using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        return _tileList.Where(t => t.GetPosition().Equals(position)).First();
    }

    public List<BaseTile> GetTilesWithinBounds(Bounds bounds)
    {

        var list = new List<BaseTile>();
        Vector2 upperRight = new Vector2(bounds.center.x + bounds.extents.x - .16f, bounds.center.y + bounds.extents.y - .16f);
        Vector2 upperLeft = new Vector2(bounds.center.x - bounds.extents.x + .16f, bounds.center.y + bounds.extents.y - .16f);
        Vector2 lowerRight = new Vector2(bounds.center.x + bounds.extents.x - .16f, bounds.center.y - bounds.extents.y + .16f);
        Vector2 lowerLeft = new Vector2(bounds.center.x - bounds.extents.x + .16f, bounds.center.y - bounds.extents.y + .16f);

        for (float x = lowerLeft.x; x < lowerRight.x; x += .32f)
        {
            for (float y = lowerLeft.y; y < upperLeft.y; y += .32f)
            {
                var pos = new Vector2(x, y);
                var tile = GetTile(pos);
                if (tile)
                {
                    list.Add(tile);
                }
            }
        }

        Transform ul = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Transform ur = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Transform ll = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Transform lr = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

        ul.localScale = ur.localScale = ll.localScale = lr.localScale = Vector3.one * .32f;

        ul.name = "Upper Left";
        ur.name = "Upper Right";
        ll.name = "Lower Left";
        lr.name = "Lower Right";

        ul.position = upperLeft;
        ur.position = upperRight;
        ll.position = lowerLeft;
        lr.position = lowerRight;


        // list = _tileList.Where(t =>
        // (t.GetPosition().x >= lowerLeft.x && t.GetPosition().x <= lowerRight.x) &&
        // (t.GetPosition().y >= lowerLeft.y && t.GetPosition().y <= upperLeft.y)
        // ).ToList();
        foreach (var item in list)
        {
            GroundTile t = (GroundTile)item;
            t.SetColor(Color.black);
        }
        return list;
    }
    internal Vector2 GetGridSize()
    {
        return GridSize;
    }
}
