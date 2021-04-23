using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Text selectedText;
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
        groundManager = floorManager.GetComponent<GroundManager>();
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
                buildingRenderer.material.color = defaultColor;
                float buildHeight = buildingRenderer.bounds.size.y / 2;
                groundManager.BuildObject(objectToBuilded, GroundManager.buildPoint, buildHeight);
                selectedText.text = "Selected: Nothing ";
            }
        }
    }
    
    public void SelectBuildingBlueprint(int id)
    {
        objectToBuilded = prefabs[id];
        selectedText.text = "Selected: " + prefabs[id].name;
        Renderer objectToBuildedRenderer = objectToBuilded.GetComponentInChildren<Renderer>();
        groundManager.boundSizeX = objectToBuildedRenderer.bounds.size.x / 2;
        groundManager.boundSizeY = objectToBuildedRenderer.bounds.size.z / 2;

        blueprintHeight = (objectToBuildedRenderer.bounds.size.y / 2) + GroundManager.buildPoint.y;
        blueprintModel = Instantiate(objectToBuilded, new Vector3(0, 100, 0), Quaternion.identity);
        blueprintModel.tag = "Blueprint";

        //if model is child
        blueprintModel.GetComponentInChildren<Transform>().tag = "Blueprint";
        blueprintModel.GetComponentInChildren<BoxCollider>().enabled = false;
        buildingRenderer = blueprintModel.GetComponentInChildren<Renderer>();
        // defaultColor = buildingRenderer.material.color;
        //
        ChangeAlpha(buildingRenderer.material, 0.3f);

        blueprintModel.layer = 9;
        blueprintSelected = true;
    }

    static public void ChangeAlpha(Material material, float alphaVal)
    {
        Color oldColor = material.color;
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
    }
}
