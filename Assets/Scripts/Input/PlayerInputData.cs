using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/Player Input Data")]
public class PlayerInputData : AbstractInputData
{
    [Header("Position Base Control")]
    [SerializeField] private bool positionBaseActive;
    [Header("Axis Base Control")]
    [SerializeField] private bool axisBaseActive;
    [SerializeField] private string axisNameHorizontal;
    [SerializeField] private string axisNameVertical;
    [Header("Key Base Control")]
    [SerializeField] private bool keyBaseActive;
    [SerializeField] private bool keyBaseHorizontalActive;
    [SerializeField] private KeyCode positiveHorizontalKeyCode;
    [SerializeField] private KeyCode negativeHorizontalKeyCode;
    [SerializeField] private bool keyBaseVerticalActive;
    [SerializeField] private KeyCode positiveVerticalKeyCode;
    [SerializeField] private KeyCode negativeVerticalKeyCode;
    [SerializeField] private float increaseAmount;
    [Header("Check Mouse Buttons")]
    [SerializeField] private bool mouseButtonsActive;
    [SerializeField] private int leftClickButton;
    [SerializeField] private int rightClickButton;


    public override void ProcessInput()
    {
        if (positionBaseActive)
        {
            Horizontal = Input.mousePosition.x;
            Vertical = Input.mousePosition.y;
        }
        else if (axisBaseActive)
        {
            Horizontal = Input.GetAxis(axisNameHorizontal);
            if (axisNameVertical.Length > 0)
            {
                Vertical = Input.GetAxis(axisNameVertical);
            }
        }
        else if (keyBaseActive)
        {
            if (keyBaseHorizontalActive)
            {
                KeyBaseAxisControl(ref Horizontal, positiveHorizontalKeyCode, negativeHorizontalKeyCode);
            }
            if (keyBaseVerticalActive)
            {
                KeyBaseAxisControl(ref Vertical, positiveVerticalKeyCode, negativeHorizontalKeyCode);
            }
        }
        else if (mouseButtonsActive)
        {
            LeftClick = Input.GetMouseButton(leftClickButton);
            RightClick = Input.GetMouseButton(rightClickButton);
        }
    }
    private void KeyBaseAxisControl(ref float value, KeyCode positive, KeyCode negative)
    {
        bool positiveActive = Input.GetKey(positive);
        bool negativeActive = Input.GetKey(negative);
        if (positiveActive)
        {
            value += increaseAmount;
        }
        else if (negativeActive)
        {
            value -= increaseAmount;
        }
        else
        {
            value = 0;
        }
    }
}
