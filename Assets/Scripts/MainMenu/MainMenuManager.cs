using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("InGameScene");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("InGameScene");
        GameManager.Instance.loadedGame = true;
    }
}
