using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public List<Selectable> AllItems = new List<Selectable>();
    public List<SavedObjectData> savedObjectDatas = new List<SavedObjectData>();
    public SelectableObjectType[] allowedTypesToMultiPlace;
    public RayingObjectType CurrentRayingObjectType;
    public bool loadedGame;
    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        loadedGame = false;
        allowedTypesToMultiPlace = new SelectableObjectType[] { SelectableObjectType.FloorTile, SelectableObjectType.Wall };
        Instance.LoadResources();
    }
    void LoadResources()
    {
        UnityEngine.Object[] materials = Resources.LoadAll("BlenderMaterials", typeof(Material)); //to be created later

        UnityEngine.Object[] buildObjects = Resources.LoadAll("BlenderModels/build_objects", typeof(GameObject));
        UnityEngine.Object[] floorObjects = Resources.LoadAll("BlenderModels/floors", typeof(GameObject));
        UnityEngine.Object[] wallObjects = Resources.LoadAll("BlenderModels/walls", typeof(GameObject));

        for (int i = 0; i < buildObjects.Length; i++)
        {
            Selectable selectable = new Selectable();
            selectable.GameObject = buildObjects[i] as GameObject;
            selectable.Type = SelectableObjectType.BuildObject;
            AllItems.Add(selectable);
        }
        for (int i = 0; i < floorObjects.Length; i++)
        {
            Selectable selectable = new Selectable();
            selectable.GameObject = floorObjects[i] as GameObject;
            selectable.Type = SelectableObjectType.FloorTile;
            selectable.MultiPlacingAllowed = true;
            AllItems.Add(selectable);
        }
        for (int i = 0; i < wallObjects.Length; i++)
        {
            Selectable selectable = new Selectable();
            selectable.GameObject = wallObjects[i] as GameObject;
            selectable.Type = SelectableObjectType.Wall;
            selectable.MultiPlacingAllowed = true;
            AllItems.Add(selectable);
        }
    }
    public GameObject GetItemGameObjectFromResources(string name)
    {
        return AllItems.FirstOrDefault(x => x.Name == name).GameObject;
    }
}
