using UnityEngine;


namespace Scripts.Bases
{

public abstract class BaseTile : MonoBehaviour
{
    [SerializeField] private bool IsWalkable;
    [SerializeField] private BaseUnit OccupiedBy;
    [SerializeField] private Vector2 Position;


    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
    public Vector2 GetPosition()
    {
        return Position;
    }
    public virtual bool GetIsWalkable()
    {
        return IsWalkable && OccupiedBy == null;
    }
    public virtual void SetOccupier(BaseUnit occupier)
    {
        OccupiedBy = occupier;
    }
    public virtual void RemoveOccupier()
    {
        OccupiedBy = null;
    }
    public virtual BaseUnit GetOccupier()
    {
        return OccupiedBy;
    }
}
}