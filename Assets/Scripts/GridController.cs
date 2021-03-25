using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    [SerializeField] private GameObject GridCube;

    public float Size { get { return size; } }

    private void Start()
    {
        CreateGridPlane();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
    private void Update()
    {
        MouseTest();
    }
    private void CreateGridPlane()
    {
        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                var cube = Instantiate(GridCube, point, Quaternion.identity);
                cube.transform.parent = this.transform;
            }
        }
    }
    private void MouseTest()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Grid")
            {
                hit.collider.GetComponent<GridBlock>().isHitting = true;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
    }
}
