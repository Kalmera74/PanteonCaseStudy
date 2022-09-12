using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierUnit : BaseUnit
{


    public void MoveTo(BaseTile tile)
    {

        SetPosition(tile.GetPosition());

    }

    public override void SetPosition(Vector2 position)
    {
        base.SetPosition(position);
        StartCoroutine(Move(transform.position, position));
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = Time.time;
        from.z = to.z = -1;

        while (Time.time - t < 1f)
        {
            transform.position = Vector3.Lerp(from, to, Time.time - t);
            yield return null;
        }
        HideSelection();
    }

    protected override void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            OnSelection();
        }
    }
}
