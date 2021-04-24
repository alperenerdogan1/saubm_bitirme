using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOT IN USE 23.04.2021
public class GridManager : MonoBehaviour
{
    public int width, height;
    [Range(0.3f, 1)]
    [SerializeField] private float size = 1f;
    public float Size { get { return size; } }

    [SerializeField] private GameObject baseGridCube;
    private GameObject gridCube;

    public static GameObject blueprintModel;

    private void Start()
    {
        gridCube = Instantiate(baseGridCube);
        CreateGridPlane();
    }

    private RaycastHit hitObject;

    private void Update()
    {
        MouseTest();
    }

    private void CreateGridPlane()
    {
        gridCube.transform.localScale *= size;
        for (float x = 0; x < height; x += size)
        {
            for (float z = 0; z < width; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                var cube = Instantiate(gridCube, point, Quaternion.identity, this.transform);
                // cube.transform.parent = this.transform;
            }
        }
        Destroy(gridCube);
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

    private void MouseTest()
    {
        RaycastHit[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(ray, Mathf.Infinity, 4);
        Debug.Log(hits.Length);
        RaycastHit hit = hits[0];
        if (hit.collider.tag == "GridBlock")
        {
            Debug.Log(hits[0].collider.tag);
            hit.collider.GetComponent<GridBlock>().isHitting = true;
            if (blueprintModel != null)
            {
                blueprintModel.transform.position = hit.point;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (InventoryController.objectToBuilded != null)
                {
                    hit.collider.GetComponent<GridBlock>().BuildObject(InventoryController.objectToBuilded);
                }
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 300, Color.yellow);
    }
}
