using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input Scriptable Objects")]
    [SerializeField] AbstractInputData buildObjectInput;
    [SerializeField] AbstractInputData blueprintRotateInput;
    [SerializeField] AbstractInputData cancelBlueprintInput;
    [Header("References")]
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] GroundManager groundManager;
    [SerializeField] BuildManager buildManager;

    void Update()
    {
        switch (inventoryManager.currentSelectionType)
        {
            case SelectionType.Nothing:
                break;
            case SelectionType.Blueprint:
                CheckBlueprintInputs();
                break;
            case SelectionType.Builded:
                break;
            default:
                break;
        }
    }

    void CheckBlueprintInputs()
    {
        inventoryManager.blueprintModel.transform.position = groundManager.buildPoint;
        if (buildObjectInput.LeftClick)
        {
            buildManager.BuildObject(inventoryManager.selectedItem, groundManager.buildPoint);
            if (!inventoryManager.selectedItem.multiPlacingAllowed)
            {
                inventoryManager.DeselectBlueprint();
            }
        }
        if (blueprintRotateInput.Horizontal < 0)
        {
            inventoryManager.blueprintModel.transform.Rotate(Vector3.up * 90, Space.Self);
        }
        if (blueprintRotateInput.Horizontal > 0)
        {
            inventoryManager.blueprintModel.transform.Rotate(Vector3.up * -90, Space.Self);
        }
        if (cancelBlueprintInput.KeyPressed)
        {
            inventoryManager.DeselectBlueprint();
        }
    }
}
