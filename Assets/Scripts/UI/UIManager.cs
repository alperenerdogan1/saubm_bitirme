using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text selectedObjectNameText;

    void Start()
    {
        selectedObjectNameText.text = "Selected: Nothing";
    }
    public void ChangeSelectedText(string selectedObjectName)
    {
        selectedObjectNameText.text = "Selected: " + selectedObjectName;

    }
}
