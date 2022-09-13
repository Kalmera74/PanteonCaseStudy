using System;
using Scripts.Bases;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{

    public class BuildingUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private BuildingData BuildingData;
        [SerializeField] private Image Image;
        [SerializeField] private GameObject Overlay;
        [SerializeField] private int ItemIndex;
        public event Action<int> OnMouseClick;
        public void OnPointerDown(PointerEventData eventData)
        {
            OnMouseClick?.Invoke(ItemIndex);
        }

        public void OnSelected()
        {
            Overlay.SetActive(true);
        }
        public void OnDeselect()
        {
            Overlay.SetActive(false);
        }
        internal void SetData(BuildingData data)
        {
            BuildingData = data;
            SetImage(data.Icon);
        }
        public BuildingData GetData()
        {
            return BuildingData;
        }
        private void SetImage(Sprite icon)
        {
            Image.sprite = icon;
        }

        public void Register(int itemIndex, Action<int> callback)
        {
            ItemIndex = itemIndex;
            OnMouseClick += callback;
        }
        public void Deregister(Action<int> callback)
        {
            OnMouseClick -= callback;
        }
    }

}