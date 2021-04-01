using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    private Color defaultColor;
    public bool isHitting = false;
    private bool isColored;

    private void Start()
    {
        defaultColor = GetComponent<MeshRenderer>().material.color;
    }

    private void Update()
    {
        colorHoveredGrid();

    }

    public void ColorThis(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
    public void SetDefaultColor()
    {
        GetComponent<Renderer>().material.color = defaultColor;
    }

    private void colorHoveredGrid()
    {
        if (isHitting)
        {
            if (!isColored)
            {
                ColorThis(new Color(0, 255, 0));
                isColored = true;
            }
        }
        else
        {
            if (isColored)
            {
                SetDefaultColor();
                isColored = false;
            }
        }
        isHitting = false;
    }
    public void BuildObject(GameObject prefab)
    {
        Vector3 pos = this.transform.position + new Vector3(0, transform.localScale.y, 0);
        var instantiated = Instantiate(prefab, pos, Quaternion.identity);
        Destroy(GridManager.blueprintModel);
        InventoryController.objectToBuilded = null;
        InventoryController.blueprintSelected = false;
    }
}
