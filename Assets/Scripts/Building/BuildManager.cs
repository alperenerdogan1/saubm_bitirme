using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject SavedObjects;
    public GameObject BuildObject(Buildable selectedItem, Vector3 buildPoint)
    {
        var instantiated = Instantiate(selectedItem.GameObject, buildPoint, selectedItem.GameObject.transform.rotation);
        instantiated.transform.parent = SavedObjects.transform;
        instantiated.name = selectedItem.Name;
        instantiated.gameObject.tag = "BuildedObject";
        instantiated.gameObject.layer = 10;
        instantiated.gameObject.AddComponent<MeshCollider>();
        var bc = instantiated.gameObject.GetComponent<BuildingController>();
        bc.AddOutline();
        return instantiated;
    }
    public GameObject BuildObject(GameObject selectedItem, Vector3 buildPoint, string name)
    {
        var instantiated = Instantiate(selectedItem, buildPoint, selectedItem.transform.rotation);
        instantiated.transform.parent = SavedObjects.transform;
        instantiated.name = name;
        instantiated.gameObject.tag = "BuildedObject";
        instantiated.gameObject.layer = 10;
        if (instantiated.gameObject.GetComponent<MeshCollider>() == null)
        {
            instantiated.gameObject.AddComponent<MeshCollider>();
        }
        if (instantiated.gameObject.GetComponent<BuildingController>() == null)
        {
            instantiated.gameObject.AddComponent<BuildingController>();
        }
        var bc = instantiated.gameObject.GetComponent<BuildingController>();
        bc.AddOutline();
        return instantiated;
    }

}
