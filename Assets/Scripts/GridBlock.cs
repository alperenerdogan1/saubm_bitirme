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
        if (isHitting)
        {
            if (!isColored)
            {
                ColorThis();
                isColored = true;
            }
        }
        else
        {
            if (isColored)
            {
                ResetColor();
                isColored = false;
            }
        }
        isHitting = false;

    }
    public void ColorThis()
    {
        GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }
    public void ResetColor()
    {
        GetComponent<Renderer>().material.color = defaultColor;
    }
}
