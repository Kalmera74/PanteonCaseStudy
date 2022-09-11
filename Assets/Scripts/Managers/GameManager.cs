using System;
using UnityEngine;

public enum GameState
{
    Init,
    GenerateGrid,
    AdjustCamera,
    Idle,
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState GameState;
    [SerializeField] private GridManager GridManager;
    [SerializeField] private UnitManager UnitManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private CameraManager CameraManager;

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
            default:
                break;
        }
    }

    private void PopulateBuildingsUI()
    {
        var buildingList = UnitManager.GetBuildingsList();
        UIManager.ShowBuildingUnits(buildingList);
    }

    private void IdleState()
    {

    }

    private void AdjustCameraState()
    {
        var bounds = GridManager.GetBounds();
        CameraManager.AdjustCamera(bounds);
    }

    private void GenerateGridState()
    {
        GridManager.GenerateGrid();
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
