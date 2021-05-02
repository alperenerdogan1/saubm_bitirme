using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject BuildObject(GameObject prefab, Vector3 buildPoint)
    {
        var instantiated = Instantiate(prefab, buildPoint, Quaternion.identity);
        var renderer = instantiated.GetComponentInChildren<Renderer>();
        CustomUtility.ChangeAlpha(renderer.material, 1);
        renderer.material = InventoryController.materials[Random.Range(1, 4)] as Material;
        return instantiated;
    }
}
