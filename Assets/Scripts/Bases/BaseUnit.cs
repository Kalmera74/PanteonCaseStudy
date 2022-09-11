using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class BaseUnit : MonoBehaviour
{
    [SerializeField] private Sprite Icon;
    [SerializeField] private List<BaseTile> Occupies;
    [SerializeField] private Vector2 Size;
    [SerializeField] private BoxCollider2D BoxCollider;
    protected virtual void Awake()
    {
        if (BoxCollider == null)
        {
            BoxCollider = GetComponent<BoxCollider2D>();
        }
        float size = 1;
        if (Size.x + Size.y > 2)
        {
            size = (Size.x * 32) / 100.0f;
        }
        BoxCollider.size = new Vector2(size, size);
    }
    public void SetOccupiedTiles(List<BaseTile> occupiedTiles)
    {
        Occupies = occupiedTiles;
    }

    public void RemoveOccupiedTiles()
    {
        Occupies = null;
    }

    public List<BaseTile> GetOccupiedTiles()
    {
        return Occupies;
    }

    public Vector2 GetSize()
    {
        return Size;
    }
    public Sprite GetIcon()
    {
        return Icon;
    }

}
