using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private AbstractInputData[] inputDataArray;
    private void Update()
    {
        for (int i = 0; i < inputDataArray.Length; i++)
        {
            inputDataArray[i].ProcessInput();
        }
    }
}
