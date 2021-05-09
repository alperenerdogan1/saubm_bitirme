using System;
using UnityEngine;

public enum SelectableObjectType { BuildObject, FloorTile, Wall };
public enum SelectionType { Nothing, Blueprint, Builded };
public enum RayingObjectType { BuildedObject, Ground };
public enum GridSizeOptions { buildObjectBased, floorBased, wallBased };
[Serializable]
public class Selectable
{
    public int Id;
    public string Name;
    private GameObject gameObject;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; string[] seperated = value.name.Split('_'); Name = seperated[0]; Int32.TryParse(seperated[1], out Id); }
    }
    public SelectableObjectType Type;
    public bool MultiPlacingAllowed = false;
}
public class Blueprint
{
    public int Id;
    public string Name;
    private GameObject gameObject;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; Renderer = gameObject.GetComponent<Renderer>(); gameObject.tag = "Blueprint"; }
    }
    public float HalfBoundSizeX;
    public float HalfBoundSizeY;
    private Renderer renderer;
    public Renderer Renderer
    {
        get { return renderer; }
        set { renderer = value; HalfBoundSizeX = renderer.bounds.size.x / 2; HalfBoundSizeY = renderer.bounds.size.z / 2; }
    }
    public SelectableObjectType Type;
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
public class Buildable
{
    public int Id;
    public string Name;
    private GameObject gameObject;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; Renderer = gameObject.GetComponent<Renderer>(); }
    }
    private Renderer renderer;
    public Renderer Renderer
    {
        get { return renderer; }
        set { renderer = value; halfBoundSizeX = renderer.bounds.size.x / 2; halfBoundSizeY = renderer.bounds.size.z / 2; }
    }
    public SelectableObjectType selectableObjectType;
    public float halfBoundSizeX;
    public float halfBoundSizeY;
    public void ChangeTransform(Vector3 point)
    {
        this.gameObject.transform.position = point;
    }
    public void RotateBlueprint(float axis)
    {
        this.gameObject.transform.Rotate(0, axis, 0);
        float temp = halfBoundSizeX;
        halfBoundSizeX = halfBoundSizeY;
        halfBoundSizeY = temp;
    }
}

[System.Serializable]
public class SavedObjectData
{
    public float[] position = new float[3];
    public float[] rotation = new float[4];
    public float[] scale = new float[3];
    public string name;
    public int id;

}