using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    private static MasterManager instance;
    public static MasterManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] private ScriptableGameSettings scriptableGameSettings;
    public ScriptableGameSettings GameSettings { get { return scriptableGameSettings; } }

}
