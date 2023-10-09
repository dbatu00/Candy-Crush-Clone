using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Name of the level selection scene
    public string levelSelectionSceneName;

    public void StartLevelSelection()
    {
        SceneManager.LoadScene(levelSelectionSceneName);
    }
}