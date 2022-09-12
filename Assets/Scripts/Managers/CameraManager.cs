using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;

    public static CameraManager Instance;
    private void Awake()
    {
        Instance = this;
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }
    public void AdjustCamera(Bounds bound)
    {

        bound.Expand(.15f);
        var vertical = bound.size.y;
        var horizontal = bound.size.x * MainCamera.pixelHeight / MainCamera.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * .5f;
        var center = bound.center + new Vector3(0, 0, -10);

        MainCamera.transform.position = center;
        MainCamera.orthographicSize = size;
    }
}
