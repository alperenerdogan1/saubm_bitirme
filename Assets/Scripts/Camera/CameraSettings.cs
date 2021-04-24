using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [SerializeField] private float positionOffset;
    public float PositionOffset { get { return positionOffset; } }
    [SerializeField] private float leftPositionOffset;
    public float LeftPositionOffset { get { return leftPositionOffset; } }
    [SerializeField] private float rightPositionOffset;
    public float RightPositionOffset { get { return rightPositionOffset; } }
    [SerializeField] private float rotationOffset;
    public float RotationOffset { get { return rotationOffset; } }
    [SerializeField] private CursorLockMode cursorLockMode;
    public CursorLockMode CursorLockMode { get { return cursorLockMode; } }
    [Header("Camera Position Settings")]
    [SerializeField] private float horizontalPositionSpeed;
    public float HorizontalPositionSpeed { get { return horizontalPositionSpeed; } }
    [SerializeField] private float verticalPositionSpeed;
    public float VerticalPositionSpeed { get { return verticalPositionSpeed; } }
    [Header("Camera Rotation Settings")]
    [SerializeField] private float rotationSpeed;
    public float RotationSpeed { get { return rotationSpeed; } }
    [Header("Camera Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    public float ZoomSpeed { get { return zoomSpeed; } }
    [SerializeField] private float zoomOutEdge;
    public float ZoomOutEdge { get { return zoomOutEdge; } }
    [SerializeField] private float zoomInEdge;
    public float ZoomInEdge { get { return zoomInEdge; } }


}
