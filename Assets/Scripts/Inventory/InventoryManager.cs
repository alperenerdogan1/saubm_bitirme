using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
    // Selected object properties
    public SelectableObjectType selectedObjectType;
    public SelectionType currentSelectionType;
    public Buildable blueprint;
    public GridSizeOptions gridSizeOptions = GridSizeOptions.buildObjectBased;
    public Selectable selectedItem;
    [Header("References")]
    // References for other managers
    [SerializeField] private UIManager uIManager;
    private void Start()
    {
        currentSelectionType = SelectionType.Nothing;
    }
    public void SelectBlueprint(int id)
    {
        selectedItem = GameManager.Instance.AllItems.First(x => x.Id == id);
        switch (selectedItem.Type)
        {
            case SelectableObjectType.BuildObject:
                selectedObjectType = SelectableObjectType.BuildObject;
                break;
            case SelectableObjectType.FloorTile:
                selectedObjectType = SelectableObjectType.FloorTile;
                break;
            case SelectableObjectType.Wall:
                selectedObjectType = SelectableObjectType.Wall;
                break;
            default:
                break;
        }
        switch (selectedObjectType)
        {
            case SelectableObjectType.BuildObject:
                gridSizeOptions = GridSizeOptions.buildObjectBased;
                break;
            case SelectableObjectType.FloorTile:
                gridSizeOptions = GridSizeOptions.floorBased;
                break;
            case SelectableObjectType.Wall:
                gridSizeOptions = GridSizeOptions.wallBased;
                break;
            default:
                gridSizeOptions = GridSizeOptions.buildObjectBased;
                break;
        }
        blueprint = new Buildable();
        blueprint.Id = selectedItem.Id;
        blueprint.Name = selectedItem.Name;
        blueprint.GameObject = Instantiate(selectedItem.GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        blueprint.GameObject.name = selectedItem.GameObject.name;
        blueprint.GameObject.AddComponent<BuildingController>();
        currentSelectionType = SelectionType.Blueprint;
    }

    public void DeselectBlueprint()
    {
        Destroy(blueprint.GameObject);
        uIManager.ChangeSelectedText("Nothing");
        currentSelectionType = SelectionType.Nothing;
    }
    public void DeselectBuilding()
    {
        uIManager.ChangeSelectedText("Nothing");
        currentSelectionType = SelectionType.Nothing;
    }
}
