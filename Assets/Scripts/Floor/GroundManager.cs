using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject GroundObject;
    [SerializeField] public float width, height, yLength;
    [SerializeField] private float size;
    public float boundSizeX, boundSizeY;
    public static Vector3 buildPoint;
    [SerializeField] private LayerMask buildPointRayLayerMask;
    void Start()
    {
        GameObject ground = Instantiate(GroundObject, new Vector3(width / 2, 0, height / 2), Quaternion.identity, this.transform);

        Vector3 appliedScale = ground.transform.localScale;
        appliedScale.x = width;
        appliedScale.y = yLength;
        appliedScale.z = height;
        ground.transform.localScale = appliedScale;

        ground.layer = MasterManager.Instance.GameSettings.GroundLayer;
    }

    void Update()
    {
        GridMousePosition();
    }

    void GridMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildPointRayLayerMask))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Ground")
            {
                Vector3 appliedPoint = hit.point;
                if (hit.point.x + boundSizeX > width)
                {
                    appliedPoint.x = width - boundSizeX;
                }
                if (hit.point.x - boundSizeX < 0)
                {
                    appliedPoint.x = boundSizeX;
                }
                if (hit.point.z + boundSizeY > height)
                {
                    appliedPoint.z = height - boundSizeY;
                }
                if (hit.point.z - boundSizeY < 0)
                {
                    appliedPoint.z = boundSizeY;
                }
                appliedPoint.y = yLength;
                buildPoint = appliedPoint;
            }
        }
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int x = Mathf.RoundToInt(position.x / size);
        int y = Mathf.RoundToInt(position.y / size);
        int z = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)x * size + (size / 2), (float)y * size, (float)z * size + (size / 2));

        result += transform.position;

        return result;
    }
    public GameObject BuildObject(GameObject prefab, Vector3 position, float buildHeight)
    {
        position += new Vector3(0, buildHeight, 0);
        var instantiated = Instantiate(prefab, position, Quaternion.identity);
        var renderer = instantiated.GetComponentInChildren<Renderer>();
        InventoryController.ChangeAlpha(renderer.material, 1);
        return instantiated;
    }
}
