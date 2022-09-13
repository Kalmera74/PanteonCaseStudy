using System.Collections;
using Scripts.Bases;
using UnityEngine;

namespace Scripts.Units
{
    public class SoldierUnit : BaseUnit
    {


        public void MoveTo(BaseTile tile)
        {

            SetPosition(tile);

        }

        public override void SetPosition(BaseTile tile)
        {
            base.SetPosition(tile);
            StartCoroutine(Move(transform.position, tile));
        }

        private IEnumerator Move(Vector3 from, BaseTile target)
        {
            float t = Time.time;
            Vector3 to = target.GetPosition();
            from.z = to.z = -1;

            while (Time.time - t < 1f)
            {
                transform.position = Vector3.Lerp(from, to, Time.time - t);
                yield return null;
            }
            target.SetOccupier(this);
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
}