using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    Outline Outline;
    public bool isHitting = false;
    public bool isSelected = false;
    public bool highlighted = false;
    private void Start()
    {
    }
    private void Update()
    {
        if (isHitting || isSelected)
        {
            if (!highlighted)
            {
                OpenHighlight();
            }
        }
        else
        {
            if (highlighted)
            {
                CloseHighlight();
                highlighted = false;
            }
        }
        isHitting = false;
        isSelected = false;
    }
    public void AddOutline()
    {
        Outline = this.gameObject.AddComponent<Outline>();
        Outline.OutlineMode = Outline.Mode.OutlineVisible;
        Outline.OutlineColor = new Color(0, 255, 0);
        Outline.OutlineWidth = 0;
    }
    public float HalfBoundSizeX;
    public float HalfBoundSizeY;
    public void OpenHighlight()
    {
        Outline.OutlineWidth = 2;
        highlighted = true;

    }
    public void CloseHighlight()
    {
        Outline.OutlineWidth = 0;
        highlighted = false;
    }
    public void ChangeTransform(Vector3 point)
    {
        this.gameObject.transform.position = point;
    }
    public void RotateBlueprint(float axis)
    {
        this.gameObject.transform.Rotate(0, axis, 0);
        float temp = HalfBoundSizeX;
        HalfBoundSizeX = HalfBoundSizeY;
        HalfBoundSizeY = temp;
    }
}
