using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public List<Selectable> selectables = new List<Selectable>();
    public SelectableObjectType[] allowedTypesToMultiPlace;
    public UnityEngine.Object[] materials;
    // Selected object properties
    public SelectableObjectType selectedObjectType;
    public SelectionType currentSelectionType;
    public GameObject blueprintModel;
    public GridSizeOptions gridSizeOptions = GridSizeOptions.buildObjectBased;
    [HideInInspector] public GameObject objectToBuilded;
    public Selectable selectedItem;
    [Header("References")]
    // References for other managers
    [SerializeField] private UIManager uIManager;
    private void Start()
    {
        allowedTypesToMultiPlace = new SelectableObjectType[] { SelectableObjectType.FloorTile, SelectableObjectType.Wall };
        LoadResources();
        uIManager.SetupUI();
        currentSelectionType = SelectionType.Nothing;
    }

    public void SelectBlueprint(int id)
    {
        selectedItem = selectables.Find(x => x.PrefabId == id);
        switch (selectedItem.type)
        {
            case SelectableObjectType.BuildObject:
                objectToBuilded = selectedItem.GameObject;
                selectedObjectType = SelectableObjectType.BuildObject;
                break;
            case SelectableObjectType.FloorTile:
                objectToBuilded = selectedItem.GameObject;
                selectedObjectType = SelectableObjectType.FloorTile;
                break;
            case SelectableObjectType.Wall:
                objectToBuilded = selectedItem.GameObject;
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
        currentSelectionType = SelectionType.Blueprint;
        uIManager.ChangeSelectedText(selectedItem.name);
        blueprintModel = Instantiate(objectToBuilded, new Vector3(0, 0, 0), Quaternion.identity);
        blueprintModel.tag = "Blueprint";
        // blueprintModel.layer = 9;
    }
    public void DeselectBlueprint()
    {
        Destroy(blueprintModel);
        uIManager.ChangeSelectedText("Nothing");
        currentSelectionType = SelectionType.Nothing;
    }

    void LoadResources()
    {
        materials = Resources.LoadAll("BlenderMaterials", typeof(Material));
        UnityEngine.Object[] buildObjects = Resources.LoadAll("BlenderModels/build_objects", typeof(GameObject));
        UnityEngine.Object[] floorObjects = Resources.LoadAll("BlenderModels/floors", typeof(GameObject));
        UnityEngine.Object[] wallObjects = Resources.LoadAll("BlenderModels/walls", typeof(GameObject));

        for (int i = 0; i < buildObjects.Length; i++)
        {
            int local = i;
            Selectable selectable = new Selectable();
            selectable.localIndex = local;
            selectable.GameObject = buildObjects[i] as GameObject;
            selectable.type = SelectableObjectType.BuildObject;
            selectable.Renderer = (buildObjects[i] as GameObject).GetComponent<Renderer>();
            selectables.Add(selectable);
        }
        for (int i = 0; i < floorObjects.Length; i++)
        {
            int local = i;
            Selectable selectable = new Selectable();
            selectable.localIndex = local;
            selectable.GameObject = floorObjects[i] as GameObject;
            selectable.type = SelectableObjectType.FloorTile;
            selectable.Renderer = (floorObjects[i] as GameObject).GetComponent<Renderer>();
            selectable.multiPlacingAllowed = true;
            selectables.Add(selectable);
        }
        for (int i = 0; i < wallObjects.Length; i++)
        {
            int local = i;
            Selectable selectable = new Selectable();
            selectable.localIndex = local;
            selectable.GameObject = wallObjects[i] as GameObject;
            selectable.type = SelectableObjectType.Wall;
            selectable.Renderer = (wallObjects[i] as GameObject).GetComponent<Renderer>();
            selectable.multiPlacingAllowed = true;
            selectables.Add(selectable);
        }
    }
}
