using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private AbstractInputData positionInputData;
    [SerializeField] private AbstractInputData rotationInputData;
    [SerializeField] private AbstractInputData zoomInputData;
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform rotationTarget;
    private bool lockCameraFlow = false;
    private float leftEdge, rightEdge, bottomEdge, upperEdge;
    void Start()
    {
        Cursor.lockState = cameraSettings.CursorLockMode;
        leftEdge = bottomEdge = 0 + cameraSettings.PositionOffset;
        rightEdge = MasterManager.Instance.GameSettings.ScreenWidth - cameraSettings.PositionOffset;
        upperEdge = MasterManager.Instance.GameSettings.ScreenHeight - cameraSettings.PositionOffset;
    }

    void Update()
    {
        ProcessMouseButton();
        if (lockCameraFlow)
        {
            ProcessCameraRotation();
        }
        if (!lockCameraFlow)
        {
            ProcessCameraZoom();
            ProcessCameraPosition();
        }
    }

    private void ProcessCameraRotation()
    {
        float horizontalInput = rotationInputData.Horizontal;
        float verticalInput = rotationInputData.Vertical;

        if (horizontalInput > cameraSettings.RotationOffset || horizontalInput < -cameraSettings.RotationOffset)
        {
            float sign = Mathf.Sign(rotationInputData.Horizontal);
            Vector3 eulers = Vector3.up * cameraSettings.RotationSpeed * sign * Time.deltaTime;
            rotationTarget.Rotate(eulers, Space.World);
        }
        if (verticalInput > cameraSettings.RotationOffset || verticalInput < -cameraSettings.RotationOffset)
        {
            float sign = Mathf.Sign(rotationInputData.Vertical);
            Vector3 eulers = Vector3.left * cameraSettings.RotationSpeed * sign * Time.deltaTime;
            rotationTarget.Rotate(eulers, Space.Self);
        }
    }

    private void ProcessCameraPosition()
    {

        Vector3 appliedPosition = positionTarget.position;
        float horizontalPositionInput = positionInputData.Horizontal;
        float verticalPositionInput = positionInputData.Vertical;
        float positionalSpeed = positionTarget.position.y / 10;
        if (horizontalPositionInput >= rightEdge || horizontalPositionInput <= leftEdge)
        {
            float sign = horizontalPositionInput <= leftEdge ? -1 : 1;
            appliedPosition += cameraSettings.HorizontalPositionSpeed * sign * Time.deltaTime * positionTarget.TransformDirection(Vector3.right) * positionalSpeed;
        }
        if (verticalPositionInput >= upperEdge || verticalPositionInput <= bottomEdge)
        {
            float sign = verticalPositionInput <= bottomEdge ? -1 : 1;
            appliedPosition += cameraSettings.VerticalPositionSpeed * sign * Time.deltaTime * positionTarget.TransformDirection(Vector3.forward) * positionalSpeed;
        }
        appliedPosition.y = positionTarget.position.y;
        positionTarget.position = appliedPosition;
    }

    private void ProcessCameraZoom()
    {
        Vector3 appliedZoomPosition = positionTarget.position;
        float zoomInput = zoomInputData.Horizontal;
        if (zoomInput > 0f)//ZoomIn
        {
            appliedZoomPosition += positionTarget.TransformDirection(Vector3.forward) * Time.deltaTime * cameraSettings.ZoomSpeed;
        }
        else if (zoomInput < 0f)//ZoomOut
        {
            appliedZoomPosition -= positionTarget.TransformDirection(Vector3.forward) * Time.deltaTime * cameraSettings.ZoomSpeed;
        }
        if (appliedZoomPosition.y < cameraSettings.ZoomInEdge || appliedZoomPosition.y > cameraSettings.ZoomOutEdge)
        {
            appliedZoomPosition = positionTarget.position;
        }
        positionTarget.position = appliedZoomPosition;
    }

    private void ProcessMouseButton()
    {
        if (Input.GetMouseButtonUp(1))
        {
            lockCameraFlow = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            lockCameraFlow = true;
        }
    }
}
