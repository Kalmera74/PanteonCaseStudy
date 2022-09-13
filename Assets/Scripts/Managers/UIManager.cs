
using System.Collections.Generic;
using Scripts.Bases;
using Scripts.UI;
using Scripts.Units;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Managers
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform ScrollViewContent;
        [SerializeField] private GameObject BuildingUIPrefab;
        [SerializeField] private GameObject ProducedUIPrefab;
        [SerializeField] private Text BuildingNameText;
        [SerializeField] private Image BuildingIcon;
        [SerializeField] private GameObject InfoParent;
        [SerializeField] private GameObject ProduceParent;
        [SerializeField] private Transform ProduceGridParent;
        [SerializeField] private List<BuildingUI> BuildingUIs = new List<BuildingUI>();
        private BuildingUI _selectedBuildingUI;
        private int _selectedIndex;
        public static UIManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public int GetSelectedItemIndex()
        {
            return _selectedIndex;
        }
        public void ShowBuildingUnits(List<BaseBuilding> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                BuildingData data = buildings[i].GetBuildingData();
                var obj = ObjectPoolingManager.Instance.Spawn(BuildingUIPrefab, Vector3.zero, Quaternion.identity);
                BuildingUI building = obj.GetComponent<BuildingUI>();
                BuildingUIs.Add(building);
                building.transform.SetParent(ScrollViewContent);
                building.SetData(data);
                building.Register(i, ItemSelected);

            }
        }

        public void ShowBuildingInfo(BuildingData data)
        {
            ShowInfoPanel();
            ClearGrid();
            BuildingNameText.text = data.Name;
            BuildingIcon.sprite = data.Icon;

            if (!data.GetType().Equals(typeof(BarrackBuildingData)))
            {
                return;
            }

            var barrackBuildingData = (BarrackBuildingData)data;
            var units = barrackBuildingData.Units;

            if (units != null)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    var unit = units[i];

                    GameObject imageObj = ObjectPoolingManager.Instance.Spawn(ProducedUIPrefab, Vector3.zero, Quaternion.identity);
                    imageObj.transform.SetParent(ProduceGridParent);
                    Image image = imageObj.GetComponent<Image>();
                    image.sprite = unit.GetIcon();
                }
            }

        }

        private void ShowInfoPanel()
        {
            InfoParent.SetActive(true);
        }
        public void HideInfoPanel()
        {
            InfoParent.SetActive(false);
        }

        private void ClearGrid()
        {

            foreach (Transform child in ProduceGridParent.transform)
            {
                ObjectPoolingManager.Instance.Destroy(child.gameObject);
            }

        }

        private void ItemSelected(int index)
        {
            if (_selectedBuildingUI == null)
            {
                _selectedBuildingUI = BuildingUIs[index];
            }
            _selectedBuildingUI.OnDeselect();
            _selectedBuildingUI = BuildingUIs[index];
            _selectedIndex = index;
            HideInfoPanel();
            _selectedBuildingUI.OnSelected();
            UnitManager.Instance.DeselectSelectedUnit();
            GameManager.Instance.SetState(GameState.BuildingSelectedToSpawn);

        }
        public void SpawnBuilding(BaseTile selectedTile)
        {
            if (_selectedBuildingUI != null)
            {

                UnitManager.Instance.SpawnBuilding(selectedTile, _selectedIndex);
                DeselectBuildingUI();
                GameManager.Instance.SetState(GameState.BuildingSpawned);
            }
        }
        private void DeselectBuildingUI()
        {
            _selectedBuildingUI.OnDeselect();
            _selectedBuildingUI = null;
        }
    }
}