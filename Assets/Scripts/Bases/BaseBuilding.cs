using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData
{
    public string Name;
    public Sprite Icon;

}

public abstract class BaseBuilding : BaseUnit
{
    [SerializeField] protected BaseTile SpawnPoint;

    [SerializeField] protected Transform SpriteRendererTransform;
    [SerializeField] protected string BuildingName;

    protected override void Awake()
    {
        base.Awake();
        if (SpriteRendererTransform == null)
        {
            SpriteRendererTransform = GetComponentInChildren<SpriteRenderer>().transform;
        }
        SpriteRendererTransform.localScale = new Vector3(GetSize().x, GetSize().y, 1);
    }

    public virtual BuildingData GetBuildingData()
    {
        return new BuildingData
        {
            Name = this.BuildingName,
            Icon = this.GetIcon()
        };
    }

    public virtual void SetSpawnPoint(BaseTile spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }

  
}
