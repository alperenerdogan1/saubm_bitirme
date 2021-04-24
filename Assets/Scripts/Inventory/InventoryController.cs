using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Object[] prefabs;
    public static Object[] materials;
    
    [SerializeField] private Text selectedText;
    public Button buildObjectButton;
    public LayoutGroup buildObjectButtonLayoutGorup;
    private List<GameObject> scaledPrefabs;

    public static bool blueprintSelected = false;
    public static GameObject blueprintModel;
    public static GameObject objectToBuilded;
    Color blueprintColor, defaultColor;
    float blueprintHeight;
    [SerializeField]
    private GameObject floorManager;
    private GroundManager groundManager;
    Renderer buildingRenderer;
    private void Start()
    {
        materials = Resources.LoadAll("BlenderMaterials", typeof(Material));
        SetupUI();
        groundManager = floorManager.GetComponent<GroundManager>();
        blueprintHeight = groundManager.yLength / 2;
    }

    private void Update()
    {
        if (blueprintSelected)
        {
            blueprintModel.transform.position = GroundManager.buildPoint + new Vector3(0, blueprintHeight, 0);
            if (Input.GetMouseButtonDown(0))
            {
                blueprintSelected = false;
                Destroy(blueprintModel);
                groundManager.BuildObject(objectToBuilded, GroundManager.buildPoint);
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
        blueprintSelected = true;
    }

    static public void ChangeAlpha(Material material, float alphaVal)
    {
        Color oldColor = material.color;
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
    }
    void SetupUI()
    {
        prefabs = Resources.LoadAll("BlenderModels", typeof(GameObject));
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

}
