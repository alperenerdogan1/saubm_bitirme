using UnityEngine;


public class BuildManager : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;
    public GameObject BuildObject(Selectable selectedItem, Vector3 buildPoint)
    {
        var instantiated = Instantiate(selectedItem.GameObject, buildPoint, Quaternion.identity);
        // var renderer = instantiated.GetComponentInChildren<Renderer>();
        // if (selectedItem.type == SelectableObjectType.BuildObject) renderer.material = inventoryManager.materials[Random.Range(1, 4)] as Material;
        return instantiated;
    }
}
