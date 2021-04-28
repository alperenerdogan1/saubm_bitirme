using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryController : MonoBehaviour
{
    public enum SelectedObjectType { Nothing, Blueprint, BuildObject, FloorTile, Wall };
    //Loading from resources
    public Object[] prefabs;
    public static Object[] materials;
    // for testing
    [SerializeField] private Text selectedText;
    //UI
    public Button buildObjectButton;
    public LayoutGroup buildObjectButtonLayoutGorup;
    // Selected object properties
    SelectedObjectType selectedObject;
    public float selectedObjectBoundSizeX, selectedObjectBoundSizeY;
    public GameObject blueprintModel;
    public GameObject objectToBuilded;
    Renderer buildingRenderer;
    // References for other managers
    [SerializeField]
    private GameObject floorManager;
    //References for scripts of other managers
    private GroundManager groundManager;
    private void Start()
    {
        LoadResources();
        SetupUI();
        groundManager = floorManager.GetComponent<GroundManager>();
        selectedObject = SelectedObjectType.Nothing;
    }

    private void Update()
    {
        if (selectedObject == SelectedObjectType.Blueprint)
        {
            blueprintModel.transform.position = groundManager.buildObjectPoint;
            if (Input.GetMouseButtonDown(0))
            {
                selectedObject = SelectedObjectType.Nothing;
                Destroy(blueprintModel);
                groundManager.BuildObject(objectToBuilded);
                selectedText.text = "Selected: Nothing ";
            }
        }
    }

    public void SelectBuildingBlueprint(int id)
    {
        objectToBuilded = prefabs[id] as GameObject;
        selectedText.text = "Selected: " + prefabs[id].name;
        Renderer objectToBuildedRenderer = objectToBuilded.GetComponentInChildren<Renderer>();
        groundManager.boundSizeX = objectToBuildedRenderer.bounds.size.x / 2;
        groundManager.boundSizeY = objectToBuildedRenderer.bounds.size.z / 2;
        objectToBuildedRenderer.material = materials[0] as Material;
        blueprintModel = Instantiate(objectToBuilded, new Vector3(0, 100, 0), Quaternion.identity);
        blueprintModel.tag = "Blueprint";
        buildingRenderer = blueprintModel.GetComponentInChildren<Renderer>();
        blueprintModel.layer = 9;
        selectedObject = SelectedObjectType.Blueprint;
    }

    void SetupUI()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            int localIndex = i;
            var button = Instantiate(buildObjectButton);
            button.transform.SetParent(buildObjectButtonLayoutGorup.transform);
            button.GetComponentInChildren<Text>().text = prefabs[i].name;
            button.GetComponent<Button>().onClick.AddListener(() => SelectBuildingBlueprint(localIndex));
        }
    }
    //DI trying
    void btnClicked(int index)
    {
        SelectBuildingBlueprint(index);
    }
    void LoadResources()
    {
        materials = Resources.LoadAll("BlenderMaterials", typeof(Material));
        prefabs = Resources.LoadAll("BlenderModels", typeof(GameObject));
    }
}
