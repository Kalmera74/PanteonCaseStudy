using UnityEngine;
namespace Scripts.Managers
{

    public enum GameState
    {
        Init,
        GenerateGrid,
        AdjustCamera,
        BuildingSelectedToSpawn,
        BuildingSpawned,
        UnitSelected,
        TileSelected,
        Idle,
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameState GameState;
        public static GameManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            StateMachine();
        }

        private void StateMachine()
        {
            switch (GameState)
            {
                case GameState.Init:
                    InitState();
                    break;
                case GameState.GenerateGrid:
                    GenerateGridState();
                    break;
                case GameState.AdjustCamera:
                    AdjustCameraState();
                    break;
                case GameState.Idle:
                    IdleState();
                    break;
                case GameState.BuildingSelectedToSpawn:
                    BuildingSelectedToSpawnState();
                    break;
                case GameState.BuildingSpawned:
                    BuildingSpawnedState();
                    break;
                case GameState.UnitSelected:
                    UnitSelectedState();
                    break;
                case GameState.TileSelected:
                    TileSelectedUnit();
                    break;
                default:
                    break;
            }
        }

        internal GameState GetState()
        {
            return GameState;
        }

        private void TileSelectedUnit()
        {

        }

        private void UnitSelectedState()
        {

        }

        private void BuildingSpawnedState()
        {

        }

        private void BuildingSelectedToSpawnState()
        {

        }

        private void PopulateBuildingsUI()
        {
            var buildingList = UnitManager.Instance.GetBuildingsList();
            UIManager.Instance.ShowBuildingUnits(buildingList);
        }

        private void IdleState()
        {

        }

        private void AdjustCameraState()
        {
            var bounds = GridManager.Instance.GetBounds();
            CameraManager.Instance.AdjustCamera(bounds);
        }

        private void GenerateGridState()
        {
            GridManager.Instance.GenerateGrid();
            SetState(GameState.AdjustCamera);
        }

        private void InitState()
        {
            PopulateBuildingsUI();
            SetState(GameState.GenerateGrid);
        }

        public void SetState(GameState state)
        {
            GameState = state;
        }
    }
}