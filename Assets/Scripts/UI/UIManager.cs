using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text selectedObjectNameText;
    //Selecting Menu
    public Button buildObjectButton;
    public LayoutGroup buildObjectButtonLayoutGorup;
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;
    void Start()
    {
        selectedObjectNameText.text = "Selected: Nothing";
    }
    public void ChangeSelectedText(string selectedObjectName)
    {
        selectedObjectNameText.text = "Selected: " + selectedObjectName;

    }
    public void SetupUI()
    {
        foreach (var item in inventoryManager.selectables)
        {
            var button = Instantiate(buildObjectButton);
            button.transform.SetParent(buildObjectButtonLayoutGorup.transform);
            button.GetComponentInChildren<Text>().text = item.name;
            button.GetComponent<Button>().onClick.AddListener(() => inventoryManager.SelectBlueprint(item.PrefabId));
        }
    }
}
