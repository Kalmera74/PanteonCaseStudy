using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;


namespace Scripts.Bases
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] protected Sprite Icon;
        [SerializeField] protected List<BaseTile> Occupies;
        [SerializeField] protected Vector3 Position;
        [SerializeField] protected Vector2 Size;
        [SerializeField] protected BoxCollider2D BoxCollider;
        [SerializeField] private GameObject SelectionOverlay;
        protected virtual void Awake()
        {
            if (BoxCollider == null)
            {
                BoxCollider = GetComponent<BoxCollider2D>();
            }
            float sizeX = .32f;
            float sizeY = .32f;
            if (Size.x > 1)
            {
                sizeX = (Size.x * 32) / 100.0f;
            }
            if (Size.y > 1)
            {
                sizeY = (Size.y * 32) / 100.0f;
            }
            BoxCollider.size = new Vector2(sizeX, sizeY);
            SelectionOverlay.transform.localScale = Size + Size * .2f;
        }
        public void SetOccupiedTiles(List<BaseTile> occupiedTiles)
        {
            Occupies = occupiedTiles;
            for (int i = 0; i < occupiedTiles.Count; i++)
            {
                BaseTile tile = occupiedTiles[i];
                tile.SetOccupier(this);
            }
        }

        public virtual void SetPosition(BaseTile tile)
        {
            if (tile.GetIsWalkable())
            {
                Position = tile.GetPosition();
                var occupiedTiles = new List<BaseTile>(){
                tile
            };
                SetOccupiedTiles(occupiedTiles);
            }
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
        protected void ShowSelection()
        {
            SelectionOverlay.SetActive(true);
        }
        public void HideSelection()
        {
            SelectionOverlay.SetActive(false);
        }
        protected virtual void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnSelection();
            }
        }

        protected void OnSelection()
        {
            UnitManager.Instance.SelectedUnit(this);
            ShowSelection();
            GameManager.Instance.SetState(GameState.UnitSelected);
        }

        internal Bounds GetBounds()
        {
            return BoxCollider.bounds;
        }

    }
}