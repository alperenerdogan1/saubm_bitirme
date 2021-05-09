using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject ESCMenuPanel;
    BuildingController selectedBuildingController;
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
                if (cancelBlueprintInput.KeyPressed)
                {
                    if (ESCMenuPanel.activeSelf)
                    {
                        ESCMenuPanel.SetActive(false);
                    }
                    else
                    {
                        ESCMenuPanel.SetActive(true);
                    }
                }
                break;
            case SelectionType.Blueprint:
                CheckBlueprintInputs();
                break;
            case SelectionType.Builded:
                CheckBuildingInputs();
                break;
            default:
                break;
        }
        switch (GameManager.Instance.CurrentRayingObjectType)
        {
            case RayingObjectType.Ground:
                if (inventoryManager.currentSelectionType == SelectionType.Nothing || inventoryManager.currentSelectionType == SelectionType.Builded)
                {
                    if (buildObjectInput.LeftClick)
                    {
                        if (inventoryManager.currentSelectionType == SelectionType.Builded)
                        {
                            selectedBuildingController.isSelected = false;
                        }
                        inventoryManager.currentSelectionType = SelectionType.Nothing;
                    }
                }
                break;
            case RayingObjectType.BuildedObject:
                if (inventoryManager.currentSelectionType == SelectionType.Nothing || inventoryManager.currentSelectionType == SelectionType.Builded)
                {
                    groundManager.PointingBuildingController.isHitting = true;
                    if (buildObjectInput.LeftClick)
                    {
                        selectedBuildingController = groundManager.PointingBuildingController;
                        inventoryManager.currentSelectionType = SelectionType.Builded;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void CheckBuildingInputs()
    {
        selectedBuildingController.isSelected = true;
        if (cancelBlueprintInput.KeyPressed)
        {
            inventoryManager.DeselectBuilding();
        }
    }

    void CheckBlueprintInputs()
    {
        BuildingController blueprintBuildingController = inventoryManager.blueprint.GameObject.GetComponent<BuildingController>();
        blueprintBuildingController.ChangeTransform(groundManager.buildPoint);
        if (buildObjectInput.LeftClick)
        {
            buildManager.BuildObject(inventoryManager.blueprint, groundManager.buildPoint);
            if (!inventoryManager.selectedItem.MultiPlacingAllowed)
            {
                var c = inventoryManager.blueprint.GameObject.GetComponent<BuildingController>();
                Destroy(c);
                inventoryManager.DeselectBlueprint();
            }
        }
        if (blueprintRotateInput.Horizontal < 0)
        {
            blueprintBuildingController.RotateBlueprint(90);
        }
        if (blueprintRotateInput.Horizontal > 0)
        {
            blueprintBuildingController.RotateBlueprint(-90);
        }
        if (cancelBlueprintInput.KeyPressed)
        {
            inventoryManager.DeselectBlueprint();
        }
    }

}
