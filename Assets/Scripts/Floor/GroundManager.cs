using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("Ground Object Settings")]
    [SerializeField] private GameObject GroundObjectPrefab;
    public Vector3 groundObjectSize;
    public float groundObjectHeight;
    [Header("Build Objects Settings")]
    public Vector3 buildObjectGizmosSize;
    [SerializeField] private bool ShowBuildGizmo = false;
    [Range(.05f, 3)]
    [SerializeField] private float buildObjectSize;
    [SerializeField] private Vector3 buildObjectSizeOffset;
    [SerializeField] private LayerMask IdleMask; //<-LAYERS BE IGNORED
    [Header("Floor Objects Settings")]
    [SerializeField] private bool ShowFloorGizmo = false;
    [Range(1, 3)]
    [SerializeField] private float floorTileSize;
    [SerializeField] private Vector3 floorTileSizeOffset;
    [SerializeField] private LayerMask FloorRayLayerMask; //<-LAYERS BE IGNORED
    [SerializeField] private LayerMask BlueprintLayerMask; //<-LAYERS BE IGNORED

    [Header("Wall")]
    [SerializeField] private bool ShowWallGizmo = false;
    [Range(.1f, 3)]
    [SerializeField] private float wallTileSize;
    [SerializeField] private Vector3 wallTileSizeOffset;
    [Header("Other")]
    // point where the ray shows for build objects
    public Vector3 buildPoint;
    public BuildingController PointingBuildingController;
    [Header("References")]
    public InventoryManager inventoryManager;
    void Start()
    {
        // initializing ground game object
        GameObject groundGameObject = Instantiate(GroundObjectPrefab,
        new Vector3(groundObjectSize.x / 2, groundObjectHeight, groundObjectSize.z / 2),
         Quaternion.identity, this.transform);
        CustomUtility.ChangeLocalScale(groundGameObject, groundObjectSize);
        groundGameObject.layer = MasterManager.Instance.GameSettings.GroundLayer;
        //
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 9999, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, BlueprintLayerMask))
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Plane")
            {
                var hitpoint = GridMousePosition(hit);
                GameManager.Instance.CurrentRayingObjectType = RayingObjectType.Ground;
            }
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, IdleMask))
        {
            if (hit.collider.tag == "BuildedObject")
            {
                PointingBuildingController = hit.collider.gameObject.GetComponent<BuildingController>();
                GameManager.Instance.CurrentRayingObjectType = RayingObjectType.BuildedObject;
            }
        }
    }
    public Vector3 restrictedHitPoint;
    Vector3 GridMousePosition(RaycastHit hit)
    {
        Vector3 appliedPoint = hit.point;
        if (!(inventoryManager.currentSelectionType == SelectionType.Nothing))
        {
            if (hit.point.x + inventoryManager.blueprint.halfBoundSizeX >= groundObjectSize.x)
            {
                appliedPoint.x = groundObjectSize.x - inventoryManager.blueprint.halfBoundSizeX;
            }
            if (hit.point.x - inventoryManager.blueprint.halfBoundSizeX < 0)
            {
                appliedPoint.x = inventoryManager.blueprint.halfBoundSizeX;
            }
            if (hit.point.z + inventoryManager.blueprint.halfBoundSizeY >= groundObjectSize.z)
            {
                appliedPoint.z = groundObjectSize.z - inventoryManager.blueprint.halfBoundSizeY;
            }
            if (hit.point.z - inventoryManager.blueprint.halfBoundSizeY < 0)
            {
                appliedPoint.z = inventoryManager.blueprint.halfBoundSizeY;
            }
        }
        restrictedHitPoint = appliedPoint;
        switch (inventoryManager.gridSizeOptions)
        {
            case GridSizeOptions.floorBased:
                appliedPoint = GetNearestPointOnGrid(appliedPoint, floorTileSize, floorTileSizeOffset);
                appliedPoint.y = groundObjectSize.y / 2 + .0001f;
                break;
            case GridSizeOptions.buildObjectBased:
                appliedPoint = GetNearestPointOnGrid(appliedPoint, buildObjectSize);
                appliedPoint.y = groundObjectSize.y / 2;
                break;
            case GridSizeOptions.wallBased:
                appliedPoint = GetNearestPointOnGrid(appliedPoint, wallTileSize, Vector3.zero);
                appliedPoint.y = groundObjectSize.y / 2;
                break;
            default:
                break;
        }
        buildPoint = appliedPoint;
        return appliedPoint;
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position, float size, bool showLogs = true)
    {
        position -= transform.position;
        int x = Mathf.RoundToInt(position.x / size);
        int y = Mathf.RoundToInt(position.y / size);
        int z = Mathf.RoundToInt(position.z / size);
        Vector3 result = new Vector3((float)x * size, (float)y * size, (float)z * size);
        result += transform.position;
        return result;
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position, float size, Vector3 offset, bool showLogs = true)
    {
        position -= transform.position;
        float x = Mathf.Floor(position.x / size);
        float y = Mathf.Floor(position.y / size);
        float z = Mathf.Floor(position.z / size);
        Vector3 result = new Vector3(x * size, y * size, z * size);
        result += transform.position;
        result += offset;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(restrictedHitPoint, .008f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(buildPoint, .08f);
        Gizmos.color = Color.red;
        if (ShowBuildGizmo)
        {
            for (float x = 0; x < buildObjectGizmosSize.z - buildObjectSizeOffset.z; x += buildObjectSize)
            {
                for (float z = 0; z < buildObjectGizmosSize.x - buildObjectSizeOffset.x; z += buildObjectSize)
                {
                    Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z), buildObjectSize, showLogs: false);
                    Gizmos.DrawSphere(point, 0.005f);
                }
            }
        }
        if (ShowFloorGizmo)
        {
            for (float x = 0; x < groundObjectSize.z; x += floorTileSize)
            {
                for (float z = 0; z < groundObjectSize.x; z += floorTileSize)
                {
                    Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z), floorTileSize, floorTileSizeOffset, showLogs: false);
                    Gizmos.DrawSphere(point, 0.05f);
                }
            }
        }
        if (ShowWallGizmo)
        {
            for (float x = 0; x < groundObjectSize.z; x += wallTileSize)
            {
                for (float z = 0; z < groundObjectSize.x; z += wallTileSize)
                {
                    Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z), wallTileSize, wallTileSizeOffset, showLogs: false);
                    Gizmos.DrawSphere(point, 0.05f);
                }
            }
        }
    }
}

