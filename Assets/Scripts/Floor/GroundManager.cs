using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("Ground Object Settings")]
    [SerializeField] private GameObject GroundObjectPrefab;
    public Vector3 groundObjectSize;
    [Header("Build Objects Settings")]
    [SerializeField] private bool ShowBuildGizmo = false;
    [Range(.2f, 3)]
    [SerializeField] private float buildObjectSize;
    [SerializeField] private LayerMask buildObjectRayLayerMask;
    [Header("Floor Objects Settings")]
    [SerializeField] private bool ShowFloorGizmo = false;
    [Range(1, 3)]
    [SerializeField] private float floorTileSize;
    [SerializeField] private Vector3 floorTileSizeOffset;
    [SerializeField] private LayerMask floorTileRayLayerMask;
    public float boundSizeX, boundSizeY;
    // point where the ray shows for floor tile
    public Vector3 floorTilePoint;
    // point where the ray shows for build objects
    public Vector3 buildObjectPoint;

    void Start()
    {
        // initializing ground game object
        GameObject groundGameObject = Instantiate(GroundObjectPrefab, new Vector3(groundObjectSize.x / 2, 0, groundObjectSize.z / 2),
         Quaternion.identity, this.transform);
        CustomUtility.ChangeLocalScale(groundGameObject, groundObjectSize);
        groundGameObject.layer = MasterManager.Instance.GameSettings.GroundLayer;
        //
    }
    void Update()
    {
        RaycastHit hit;
        if (CheckRayTag(buildObjectRayLayerMask, "Ground", out hit))
        {
            var hitpoint = GridMousePosition(hit);
        }
    }
    Vector3 GridMousePosition(RaycastHit hit)
    {
        Vector3 appliedPoint = hit.point;
        if (hit.point.x + boundSizeX > groundObjectSize.x)
        {
            appliedPoint.x = groundObjectSize.x - boundSizeX;
        }
        if (hit.point.x - boundSizeX < 0)
        {
            appliedPoint.x = boundSizeX;
        }
        if (hit.point.z + boundSizeY > groundObjectSize.z)
        {
            appliedPoint.z = groundObjectSize.z - boundSizeY;
        }
        if (hit.point.z - boundSizeY < 0)
        {
            appliedPoint.z = boundSizeY;
        }

        appliedPoint = GetNearestPointOnGrid(appliedPoint, floorTileSize, floorTileSizeOffset);
        appliedPoint.y = groundObjectSize.y / 2 + .0001f;
        buildObjectPoint = appliedPoint;

        return appliedPoint;
    }
    bool CheckRayTag(LayerMask mask, string tag, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 9999, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.tag == tag) return true;
        }
        return false;
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position, float size)
    {
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / size);
        int y = Mathf.RoundToInt(position.y / size);
        int z = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)x * size, (float)y * size, (float)z * size);
        result += transform.position;

        return result;
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position, float size, Vector3 offset)
    {
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / size);
        int y = Mathf.RoundToInt(position.y / size);
        int z = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)x * size, (float)y * size, (float)z * size);
        result += offset;
        result += transform.position;

        return result;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (ShowBuildGizmo)
        {
            for (float x = 0; x < groundObjectSize.z; x += buildObjectSize)
            {
                for (float z = 0; z < groundObjectSize.x; z += buildObjectSize)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, 0f, z), buildObjectSize);
                    Gizmos.DrawSphere(point, 0.05f);
                }
            }
        }
        if (ShowFloorGizmo)
        {
            for (float x = 0; x < groundObjectSize.z; x += floorTileSize)
            {
                for (float z = 0; z < groundObjectSize.x; z += floorTileSize)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, 0f, z), floorTileSize, floorTileSizeOffset);
                    Gizmos.DrawSphere(point, 0.05f);
                }
            }
        }
    }
    public GameObject BuildObject(GameObject prefab)
    {
        var instantiated = Instantiate(prefab, buildObjectPoint, Quaternion.identity);
        var renderer = instantiated.GetComponentInChildren<Renderer>();
        CustomUtility.ChangeAlpha(renderer.material, 1);
        renderer.material = InventoryController.materials[Random.Range(1, 4)] as Material;
        return instantiated;
    }
}
