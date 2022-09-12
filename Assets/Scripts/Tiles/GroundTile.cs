using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    [SerializeField] private SpriteRenderer TileRenderer;
    [SerializeField] private Color EvenTileColor;
    [SerializeField] private Color OddTileColor;
    public event Action<BaseTile> OnTileSelected;
    public event Action<BaseTile> OnPointerOver;
    public void SetColorByPosition(Vector2 pos)
    {
        bool isEvenTile = pos.x % 2 == 0 && pos.y % 2 != 0 || pos.x % 2 != 0 && pos.y % 2 == 0;

        if (isEvenTile)
        {
            TileRenderer.color = EvenTileColor;
        }
        else
        {
            TileRenderer.color = OddTileColor;
        }
    }
    private void OnMouseDown()
    {
        UIManager.Instance.HideInfoPanel();
        OnTileSelected?.Invoke(this);
        UnitManager.Instance.DeselectSelectedUnit();
        GameManager.Instance.SetState(GameState.TileSelected);
    }
    private void OnMouseOver()
    {
        OnPointerOver?.Invoke(this);
    }

    internal void SetColor(Color color)
    {
        TileRenderer.color = color;
    }
}
