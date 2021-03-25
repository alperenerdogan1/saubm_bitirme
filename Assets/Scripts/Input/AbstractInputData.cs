using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInputData : ScriptableObject
{
    [Header("Read Values")]
    [ReadOnlyInInspector] public bool LeftClick;
    [ReadOnlyInInspector] public bool RightClick;
    [ReadOnlyInInspector] public float Horizontal;
    [ReadOnlyInInspector] public float Vertical;
    public abstract void ProcessInput();
}
