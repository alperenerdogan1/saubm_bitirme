using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
    public enum SelectableObjectType { BuildObject, FloorTile, Wall };
    public enum SelectionType { Nothing, Blueprint, Builded };
    public enum GridSizeOptions { buildObjectBased, floorBased };

    [Serializable]
    public class Selectable
    {
        private static int Index = 0;
        public Selectable()
        {
            PrefabId = Index;
            materialId = 0;
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
        public int materialId;
        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; name = value.name; }
        }
        public SelectableObjectType type;
        public bool multiPlacingAllowed = false;
    }
    [SerializeField] public List<Selectable> selectables = new List<Selectable>();
    public static UnityEngine.Object[] materials;
    // for testing
    [SerializeField] private Text selectedText;
    //UI
    public Button buildObjectButton;
    public LayoutGroup buildObjectButtonLayoutGorup;
    // Selected object properties
    SelectableObjectType selectedObjectType;
    SelectionType currentSelectionType;
    public float selectedObjectBoundSizeX, selectedObjectBoundSizeY;
    public GameObject blueprintModel;
    [HideInInspector] public GameObject objectToBuilded;
    public GridSizeOptions gridSizeOptions = GridSizeOptions.buildObjectBased;
    public Selectable selectedItem;
    [Header("References")]
    // References for other managers
    [SerializeField] private GroundManager groundManager;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private UIManager uIManager;
    private void Start()
    {
        LoadResources();
        SetupUI();
        currentSelectionType = SelectionType.Nothing;
    }

    private void Update()
    {
        if (currentSelectionType == SelectionType.Blueprint)
        {
            blueprintModel.transform.position = groundManager.buildPoint;
            if (Input.GetMouseButtonDown(0))
            {
                buildManager.BuildObject(objectToBuilded, groundManager.buildPoint);
                if (!(selectedItem.type == SelectableObjectType.FloorTile))
                {
                    DeselectBlueprint();
                }
            }
        }
    }
    public void SelectBuildingBlueprint(int id)
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
            default:
                break;
        }
        currentSelectionType = SelectionType.Blueprint;
        uIManager.ChangeSelectedText(selectedItem.name);
        blueprintModel = Instantiate(objectToBuilded, new Vector3(0, 0, 0), Quaternion.identity);
        blueprintModel.tag = "Blueprint";
        blueprintModel.layer = 9;
    }
    void DeselectBlueprint()
    {
        Destroy(blueprintModel);
        uIManager.ChangeSelectedText("Nothing");
        currentSelectionType = SelectionType.Nothing;
    }
    void SetupUI()
    {
        foreach (var item in selectables)
        {
            var button = Instantiate(buildObjectButton);
            button.transform.SetParent(buildObjectButtonLayoutGorup.transform);
            button.GetComponentInChildren<Text>().text = item.name;
            button.GetComponent<Button>().onClick.AddListener(() => SelectBuildingBlueprint(item.PrefabId));
        }
    }
    void LoadResources()
    {
        materials = Resources.LoadAll("BlenderMaterials", typeof(Material));
        UnityEngine.Object[] buildObjects = Resources.LoadAll("BlenderModels/build_objects", typeof(GameObject));
        UnityEngine.Object[] floorObjects = Resources.LoadAll("BlenderModels/floors", typeof(GameObject));
        for (int i = 0; i < buildObjects.Length; i++)
        {
            int local = i;
            Selectable selectable = new Selectable();
            selectable.localIndex = local;
            selectable.GameObject = buildObjects[i] as GameObject;
            selectable.type = SelectableObjectType.BuildObject;
            selectable.Renderer = (buildObjects[i] as GameObject).GetComponent<Renderer>();
            selectable.Renderer.material = materials[0] as Material;
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
            selectable.Renderer.material = materials[0] as Material;
            selectable.multiPlacingAllowed = true;
            selectables.Add(selectable);
        }
    }
}
