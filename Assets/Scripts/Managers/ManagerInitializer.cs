using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    [SerializeField] private AbstractScriptableObject[] abstractScriptableObject;
    private void Start()
    {
        for (int i = 0; i < abstractScriptableObject.Length; i++)
        {
            abstractScriptableObject[i].Initialize();
        }
    }

}
