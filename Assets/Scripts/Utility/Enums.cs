using System;
using UnityEngine;

public enum SelectableObjectType { BuildObject, FloorTile, Wall };
public enum SelectionType { Nothing, Blueprint, Builded };
public enum GridSizeOptions { buildObjectBased, floorBased, wallBased };
[Serializable]
public class Selectable
{
    private static int Index = 0;
    public Selectable()
    {
        PrefabId = Index;
    }
    public int localIndex;
    public string name;
    private int prefabId;
    public int PrefabId
    {
        get
        {
            return prefabId;
        }
        set
        {
            prefabId = value;
            Index++;
        }
    }
    private Renderer renderer;
    public Renderer Renderer
    {
        get { return renderer; }
        set { renderer = value; halfBoundSizeX = value.bounds.size.x / 2; halfBoundSizeY = value.bounds.size.z / 2; }
    }
    public float halfBoundSizeX;
    public float halfBoundSizeY;
    private GameObject gameObject;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; name = value.name; }
    }
    public SelectableObjectType type;
    public bool multiPlacingAllowed = false;
    public string tag;
    public int layer;
}
class Buildable
{
    GameObject GameObject;
    public SelectableObjectType selectableObjectType;
    public float halfBoundSizeX;
    public float halfBoundSizeY;
    public string tag;
    public int layer;
    public Buildable()
    {

    }
    public void ChangeTransform()
    {

    }
    public void Rotate()
    {

    }
}