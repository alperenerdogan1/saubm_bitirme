using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class ScriptableGameSettings : AbstractScriptableObject
{
    [ReadOnlyInInspector] [SerializeField] private int screenWidth;
    public int ScreenWidth { get { return screenWidth; } }
    [ReadOnlyInInspector] [SerializeField] private int screenHeight;
    public int ScreenHeight { get { return screenHeight; } }

    public override void Initialize()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
}
